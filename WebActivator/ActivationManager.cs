using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace WebActivator {
    public class ActivationManager {
        static List<PreApplicationStartMethodAttribute> attribsToCallAfterStart = new List<PreApplicationStartMethodAttribute>();
        private static object initLock = new object();
        private static bool hasInited;

        public static void Run() {
            lock (initLock) {
                if (!hasInited) {
                    // Go through all the bin assemblies
                    foreach (var assemblyFile in GetAssemblyFiles()) {
                        var assembly = Assembly.LoadFrom(assemblyFile);

                        // Go through all the PreApplicationStartMethodAttribute attributes
                        // Note that this is *our* attribute, not the System.Web namesake
                        foreach (PreApplicationStartMethodAttribute preStartAttrib in assembly.GetCustomAttributes(
                            typeof(PreApplicationStartMethodAttribute),
                            inherit: false)) {

                            // If it asks to be called after global.asax App_Start, keep track of the method. Otherwise call it now
                            if (preStartAttrib.CallAfterGlobalAppStart && HostingEnvironment.IsHosted) {
                                attribsToCallAfterStart.Add(preStartAttrib);
                            }
                            else {
                                // Invoke the method that the attribute points to
                                preStartAttrib.InvokeMethod();
                            }
                        }
                    }

                    // If any method needs to be called later, register our module to do it
                    if (attribsToCallAfterStart.Count > 0) {
                        Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(StartMethodCallingModule));
                    }

                    hasInited = true;
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
                        attribsToCallAfterStart.ForEach(a => a.InvokeMethod());
                        hasInited = true;
                    }
                }
            }

            public void Dispose() {
            }
        }
    }
}