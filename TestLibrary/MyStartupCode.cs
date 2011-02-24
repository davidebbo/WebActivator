using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start2")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "CallMeAfterAppStart")]

namespace TestLibrary {
    public static class MyStartupCode {
        public static bool StartCalled { get; set; }
        public static bool Start2Called { get; set; }
        public static bool CallMeAfterAppStartCalled { get; set; }

        internal static void Start() {
            StartCalled = true;
        }

        public static void Start2() {
            Start2Called = true;
        }

        public static void CallMeAfterAppStart() {
            // This gets called after global.asax's Application_Start

            CallMeAfterAppStartCalled = true;
        }
    }
}
