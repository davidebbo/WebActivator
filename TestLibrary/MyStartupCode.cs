using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start2")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "CallMeAfterAppStart", callAfterGlobalAppStart: true)]

namespace TestLibrary {
    public static class MyStartupCode {
        private static bool _startCalled;
        private static bool _start2Called;
        private static bool _callMeAfterAppStartCalled;

        internal static void Start() {
            _startCalled = true;

            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Bar", // Route name
                "CoolAbout", // URL with parameters
                new { controller = "Home", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void Start2() {
            _start2Called = true;

            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Bar2", // Route name
                "CoolAbout2", // URL with parameters
                new { controller = "Home", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void CallMeAfterAppStart() {
            // This gets called after global.asax's Application_Start

            _callMeAfterAppStartCalled = true;
        }

        // Called by the unit test
        public static bool AllStartMethodsWereCalled() {
            return _startCalled && _start2Called && _callMeAfterAppStartCalled;
        }
    }
}
