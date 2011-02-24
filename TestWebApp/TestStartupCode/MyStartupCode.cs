using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestWebApp.TestStartupCode.MyStartupCode), "Start")]

namespace TestWebApp.TestStartupCode {
    public static class MyStartupCode {
        public static bool StartCalled { get; set; }

        public static void Start() {
            StartCalled = true;
        }
    }
}
