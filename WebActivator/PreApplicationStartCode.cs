using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;

namespace WebActivator {
    public class PreApplicationStartCode {
        static List<PreApplicationStartMethodAttribute> _attribsToCallAfterStart = new List<PreApplicationStartMethodAttribute>();

        public static void Start() {
            // Go through all the bin assemblies
            foreach (var assemblyFile in Directory.GetFiles(HttpRuntime.BinDirectory, "*.dll")) {
                var assembly = Assembly.LoadFrom(assemblyFile);

                // Go through all the PreApplicationStartMethodAttribute attributes
                // Note that this is *our* attribute, not the System.Web namesake
                foreach (PreApplicationStartMethodAttribute preStartAttrib in assembly.GetCustomAttributes(
                    typeof(PreApplicationStartMethodAttribute),
                    inherit: false)) {

                    // If it asks to be called after global.asax App_Start, keep track of the method. Otherwise call it now
                    if (preStartAttrib.CallAfterGlobalAppStart) {
                        _attribsToCallAfterStart.Add(preStartAttrib);
                    }
                    else {
                        // Invoke the method that the attribute points to
                        preStartAttrib.InvokeMethod();
                    }
                }
            }

            // If any method needs to be called later, register our module to do it
            if (_attribsToCallAfterStart.Count > 0) {
                Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(StartMethodCallingModule));
            }
        }

        class StartMethodCallingModule : IHttpModule {
            private static object initLock = new object();
            private static bool hasInited;

            public void Init(HttpApplication context) {
                // Make sure we only call the methods once per app domain
                lock (initLock) {
                    if (!hasInited) {
                        _attribsToCallAfterStart.ForEach(a => a.InvokeMethod());
                        hasInited = true;
                    }
                }
            }

            public void Dispose() {
            }
        }
    }
}
