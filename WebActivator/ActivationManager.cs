using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace WebActivator
{
    public class ActivationManager
    {
        private static bool _hasInited;
        private static List<Assembly> _assemblies;

        // For unit test purpose
        internal static void Reset()
        {
            _hasInited = false;
            _assemblies = null;
        }

        public static void Run()
        {
            if (!_hasInited)
            {
                RunPreStartMethods();

                // Register our module to handle any Post Start methods. But outside of ASP.NET, just run them now
                if (HostingEnvironment.IsHosted)
                {
                    Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(StartMethodCallingModule));
                }
                else
                {
                    RunPostStartMethods();
                }

                _hasInited = true;
            }
        }

        private static IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    // Cache the list of relevant assemblies, since we need it for both Pre and Post
                    _assemblies = new List<Assembly>();
                    foreach (var assemblyFile in GetAssemblyFiles())
                    {
                        try
                        {
                            // Ignore assemblies we can't load. They could be native, etc...
                            _assemblies.Add(Assembly.LoadFrom(assemblyFile));
                        }
                        catch (Win32Exception) { }
                        catch (ArgumentException) { }
                        catch (FileNotFoundException) { }
                        catch (PathTooLongException) { }
                        catch (BadImageFormatException) { }
                        catch (SecurityException) { }
                    }
                }

                return _assemblies;
            }
        }

        private static IEnumerable<string> GetAssemblyFiles()
        {
            // When running under ASP.NET, find assemblies in the bin folder.
            // Outside of ASP.NET, use whatever folder WebActivator itself is in
            string directory = HostingEnvironment.IsHosted
                ? HttpRuntime.BinDirectory
                : Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return Directory.GetFiles(directory, "*.dll");
        }

        // Return all the App_Code assemblies
        private static IEnumerable<Assembly> AppCodeAssemblies
        {
            get
            {
                // Return an empty list if we;re not hosted or there aren't any
                if (!HostingEnvironment.IsHosted || !_hasInited || BuildManager.CodeAssemblies == null)
                {
                    return Enumerable.Empty<Assembly>();
                }

                return BuildManager.CodeAssemblies.OfType<Assembly>();
            }
        }

        public static void RunPreStartMethods()
        {
            RunActivationMethods<PreApplicationStartMethodAttribute>();
        }

        public static void RunPostStartMethods()
        {
            RunActivationMethods<PostApplicationStartMethodAttribute>();
        }

        public static void RunShutdownMethods()
        {
            RunActivationMethods<ApplicationShutdownMethodAttribute>();
        }

        // Call the relevant activation method from all assemblies
        private static void RunActivationMethods<T>() where T : BaseActivationMethodAttribute
        {
            foreach (var assembly in Assemblies.Concat(AppCodeAssemblies))
            {
                foreach (BaseActivationMethodAttribute activationAttrib in assembly.GetActivationAttributes<T>().OrderBy(att => att.Order))
                {
                    activationAttrib.InvokeMethod();
                }
            }
        }

        class StartMethodCallingModule : IHttpModule
        {
            private static object _lock = new object();
            private static int _initializedModuleCount;

            public void Init(HttpApplication context)
            {

                lock (_lock)
                {
                    // Keep track of the number of modules initialized and
                    // make sure we only call the post start methods once per app domain
                    if (_initializedModuleCount++ == 0)
                    {
                        RunPostStartMethods();
                    }
                }
            }

            public void Dispose()
            {
                lock (_lock)
                {
                    // Call the shutdown methods when the last module is disposed
                    if (--_initializedModuleCount == 0)
                    {
                        RunShutdownMethods();
                    }
                }
            }
        }
    }
}
