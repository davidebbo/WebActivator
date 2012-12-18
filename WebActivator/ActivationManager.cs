using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace WebActivator
{
    public class ActivationManager
    {
        private static bool _hasInited;
        private static List<Assembly> _assemblies;
        private static List<Config> _configTasks;

        // For unit test purpose
        internal static void Reset()
        {
            _hasInited = false;
            _assemblies = null;
            _configTasks = null;
        }

        public static void Run()
        {
            if (!_hasInited)
            {
                RunPreStartMethods();

                // Register our module to handle any Post Start methods. But outside of ASP.NET, just run them now
                if (HostingEnvironment.IsHosted)
                {
                    Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(WebActivatorHttpModule));
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
                        catch
                        {
                        }
                    }
                }

                return _assemblies;
            }
        }

        private static IEnumerable<Config> ConfigTasks
        {
            get
            {
                if (_configTasks == null)
                {
                    _configTasks = new List<Config>();
                    foreach (var assembly in Assemblies.Concat(AppCodeAssemblies))
                    {
                        _configTasks.AddRange(assembly.GetConfigTasks());
                    }
                    _configTasks.Sort(ComparisonHelper.ConfigComparison);
                }

                return _configTasks;
            }
        }

        private static IEnumerable<string> GetAssemblyFiles()
        {
            // When running under ASP.NET, find assemblies in the bin folder.
            // Outside of ASP.NET, use whatever folder WebActivator itself is in
            string directory = HostingEnvironment.IsHosted
                ? HttpRuntime.BinDirectory
                : Path.GetDirectoryName(typeof(ActivationManager).Assembly.Location);
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
            foreach (Config configTask in ConfigTasks)
            {
#if DEBUG
                Debug.WriteLine("Running config task. Method: PreSetup(). Priority: {0}. Name: {1}", configTask.Priority, configTask.GetType().Name);
#endif
                configTask.PreSetup();
            }
            RunActivationMethods<PreApplicationStartMethodAttribute>();
        }

        public static void RunPostStartMethods()
        {
            foreach (Config configTask in ConfigTasks)
            {
#if DEBUG
                Debug.WriteLine("Running config task. Method: Setup(). Priority: {0}. Name: {1}", configTask.Priority, configTask.GetType().Name);
#endif
                configTask.Setup();
            }
            RunActivationMethods<PostApplicationStartMethodAttribute>();
        }

        public static void RunShutdownMethods()
        {
            foreach (Config configTask in ConfigTasks)
            {
#if DEBUG
                Debug.WriteLine("Running config task. Method: Shutdown(). Priority: {0}. Name: {1}", configTask.Priority, configTask.GetType().Name);
#endif
                configTask.Shutdown();
            }
            RunActivationMethods<ApplicationShutdownMethodAttribute>();
        }

        internal static void RunAttachEventsMethods(HttpApplication context)
        {
            foreach (Config configTask in ConfigTasks)
            {
#if DEBUG
                Debug.WriteLine("Running config task. Method: AttachEventHandlers(). Priority: {0}. Name: {1}", configTask.Priority, configTask.GetType().Name);
#endif
                configTask.AttachEventHandlers(context);
            }
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
    }
}
