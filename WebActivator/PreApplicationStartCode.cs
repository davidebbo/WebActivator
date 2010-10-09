using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.Web.Compilation;
using System.Reflection;
using System.ComponentModel.Composition;
using System.Web;
using System.IO;

namespace WebActivator {
    public class PreApplicationStartCode {
        private static bool _startWasCalled;

        public static void Start() {
            // Protect against multiple calls
            if (_startWasCalled) {
                return;
            }
            _startWasCalled = true;

            var p = new PreApplicationStartCode();
            p.Run();
        }

        public void Run() {
            Compose();

            foreach (var appStart in AppStartClasses) {
                appStart.Run();
            }
        }

        // Use MEF to find all the implementations of IApplicationStart
        private void Compose() {
            var catalog = new AggregateCatalog();

            // Go through all the bin assemblies and add them to the catalog
            foreach (var assemblyFile in Directory.GetFiles(HttpRuntime.BinDirectory, "*.dll")) {
                var assembly = Assembly.LoadFrom(assemblyFile);

                // As an optimization, skip the assemblies that don't reference our assembly, since they can't have exports we care about
                AssemblyName webActivationAssemblyName = typeof(PreApplicationStartCode).Assembly.GetName();
                if (!assembly.GetReferencedAssemblies().Any(a => AssemblyName.ReferenceMatchesDefinition(a, webActivationAssemblyName))) {
                    continue;
                }

                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        [ImportMany]
        public IEnumerable<IApplicationStart> AppStartClasses { get; set; }
    }
}
