using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using WebActivation;

namespace TestConsoleApplication {
    public class Program {
        public static void Main(string[] args) {
            Program p = new Program();
            p.Run();
        }

        public void Run() {
            Compose();
        }

        private void Compose() {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestLibrary.MyStartupCode).Assembly));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            Console.WriteLine(AppStart);
        }

        [Import]
        public IApplicationStart AppStart { get; set; }
    }
}
