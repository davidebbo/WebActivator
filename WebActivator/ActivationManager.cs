using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;

namespace WebActivatorEx
{
    public class ActivationManager
    {
        private static bool _hasInited;
        private static List<Assembly> _assemblies;
        private static Func<string, bool> _fileFilter = a => true;

        // For unit test purpose
        public static void Reset()
        {
            _hasInited = false;
            _assemblies = null;
        }

        public static void Run()
        {
            if (!_hasInited)
            {
                DetermineWhatFilesAndAssembliesToScan();

                bool isRunningMono = Type.GetType("Mono.Runtime") != null;

                if (isRunningMono)
                {
                    RunPreStartMethods(designerMode: false);
                }
                else
                {
                    // In CBM mode, pass true so that only the methods that have RunInDesigner=true get called
                    RunPreStartMethods(designerMode: IsInClientBuildManager());
                }

                // Register our module to handle any Post Start methods. But outside of ASP.NET, just run them now
                if (HostingEnvironment.IsHosted)
                {
                    Type startMethodType = typeof(StartMethodCallingModule);

                    if (isRunningMono)
                    {
                        HttpModuleActionCollection modules = (WebConfigurationManager.GetWebApplicationSection("system.web/httpModules") as HttpModulesSection).Modules;
                        modules.Add(new HttpModuleAction(startMethodType.FullName, startMethodType.AssemblyQualifiedName));
                    }
                    else
                    {
                        Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(startMethodType);
                    }
                }
                else
                {
                    RunPostStartMethods();
                }

                _hasInited = true;
            }
        }

        private static void DetermineWhatFilesAndAssembliesToScan()
        {
            var value = ConfigurationManager.AppSettings["webactivator:excludedFilesExpression"];
            if (value != null)
            {
                var fileExpression = new Regex(value.Trim());
                _fileFilter = file => !fileExpression.IsMatch(file);
            }
        }

        private static bool IsInClientBuildManager()
        {
            return HostingEnvironment.InClientBuildManager;
        }

        private static IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    // Cache the list of relevant assemblies, since we need it for both Pre and Post
                    _assemblies = new List<Assembly>();
                    foreach (var assemblyFile in GetAssemblyFiles().Where(file => _fileFilter(file)))
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

        public static void RunPreStartMethods(bool designerMode = false)
        {
            RunActivationMethods<PreApplicationStartMethodAttribute>(designerMode);
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
        private static void RunActivationMethods<T>(bool designerMode = false) where T : BaseActivationMethodAttribute
        {
            var attribs = Assemblies.Concat(AppCodeAssemblies)
                                    .SelectMany(assembly => assembly.GetActivationAttributes<T>())
                                    .OrderBy(att => att.Order);

            foreach (var activationAttrib in attribs)
            {
                // Don't run it in designer mode, unless the attribute explicitly asks for that
                if (!designerMode || activationAttrib.ShouldRunInDesignerMode())
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
