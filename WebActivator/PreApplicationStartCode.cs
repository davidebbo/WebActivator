using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace WebActivator {
    public class PreApplicationStartCode {
        public static void Start() {
            // Go through all the bin assemblies
            foreach (var assemblyFile in Directory.GetFiles(HttpRuntime.BinDirectory, "*.dll")) {
                var assembly = Assembly.LoadFrom(assemblyFile);

                // Go through all the PreApplicationStartMethodAttribute attributes
                // Note that this is *our* attribute, not the System.Web namesake
                foreach (PreApplicationStartMethodAttribute preStartAttrib in assembly.GetCustomAttributes(
                    typeof(PreApplicationStartMethodAttribute),
                    inherit: false)) {

                    // Invoke the method that the attribute points to
                    preStartAttrib.InvokeMethod();
                }
            }
        }
    }
}
