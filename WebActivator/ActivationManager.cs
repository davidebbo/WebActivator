using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System;

namespace WebActivator {
    public class ActivationManager {
        private static bool hasInited;
        private static IEnumerable<Assembly> _assemblies;

        public static void Run() {
            if (!hasInited) {
                RunPreStartMethods();

                // Register our module to handle any Post Start methods
                Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(StartMethodCallingModule));

                hasInited = true;
            }
        }

        private static IEnumerable<Assembly> Assemblies {
            get {
                if (_assemblies == null) {
                    // Cache the list of relevant assemblies, since we need it for both Pre and Post
                    _assemblies = GetAssemblyFiles().Select(file => Assembly.LoadFrom(file)).ToList();
                }

                return _assemblies;
            }
        }

        public static void RunPreStartMethods() {
            // Go through all the relevant assemblies and run the PreStart logic
            foreach (var assembly in Assemblies) {
                foreach (PreApplicationStartMethodAttribute preStartAttrib in assembly.GetPreAppStartAttributes()) {
                    // Invoke the method that the attribute points to
                    preStartAttrib.InvokeMethod();
                }
            }
        }

        public static void RunPostStartMethods() {
            // Go through all the relevant assemblies and run the PostStart logic
            foreach (var assembly in Assemblies) {
                foreach (PostApplicationStartMethodAttribute postStartAttrib in assembly.GetPostAppStartAttributes()) {
                    // Invoke the method that the attribute points to
                    postStartAttrib.InvokeMethod();
                }
            }
        }

        private static void ProcessAppCodeAssemblies() {
            // Go through all the App_Code assemblies
            foreach (var assembly in BuildManager.CodeAssemblies.OfType<Assembly>()) {
                // Fail if there are any PreStart attribs in App_Code as we can't call them since App_Code is not even compiled before App_Start.
                foreach (PreApplicationStartMethodAttribute preStartAttrib in assembly.GetPreAppStartAttributes()) {
                    throw new Exception(String.Format(
                        "PreApplicationStartMethodAttribute cannot be used in AppCode (for method {0}.{1}). Please use PostApplicationStartMethodAttribute instead.",
                        preStartAttrib.Type.FullName, preStartAttrib.MethodName));
                }

                foreach (PostApplicationStartMethodAttribute postStartAttrib in assembly.GetPostAppStartAttributes()) {
                    postStartAttrib.InvokeMethod();
                }
            }
        }

        private static IEnumerable<string> GetAssemblyFiles() {
            // When running under ASP.NET, find assemblies in the bin folder.
            // Outside of ASP.NET, use whatever folder WebActivator itself is in
            string directory = HostingEnvironment.IsHosted
                ? HttpRuntime.BinDirectory
                : Path.GetDirectoryName(typeof(ActivationManager).Assembly.Location);
            return Directory.GetFiles(directory, "*.dll");
        }

        class StartMethodCallingModule : IHttpModule {
            private static object initLock = new object();
            private static bool hasInited;

            public void Init(HttpApplication context) {

                // Make sure we only call the methods once per app domain
                lock (initLock) {
                    if (!hasInited) {
                        RunPostStartMethods();

                        // Process any attribute found in App_Code.
                        ProcessAppCodeAssemblies();

                        hasInited = true;
                    }
                }
            }

            public void Dispose() {
            }
        }
    }
}
