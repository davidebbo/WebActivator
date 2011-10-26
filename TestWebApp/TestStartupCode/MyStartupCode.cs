using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestWebApp.TestStartupCode.MyStartupCode), "Start",Order = 1)]

namespace TestWebApp.TestStartupCode {
    public static class MyStartupCode {
        public static bool StartCalled { get; set; }

        public static void Start() {
            StartCalled = true;
            Debug.WriteLine("MyStartUpCode1");
        }
    }
}
