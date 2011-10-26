using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestWebApp.TestStartupCode.MyStartupCode2), "Start",Order = 0)]

namespace TestWebApp.TestStartupCode {
    public static class MyStartupCode2 {
        public static bool StartCalled { get; set; }

        public static void Start() {
            StartCalled = true;
            Debug.WriteLine("MyStartUpCode2");
        }
    }
}
