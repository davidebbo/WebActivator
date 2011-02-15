using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "Start2")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(TestLibrary.MyStartupCode), "CallMeAfterAppStart", callAfterGlobalAppStart: true)]

namespace TestLibrary {
    static class MyStartupCode {
        internal static void Start() {
            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Bar", // Route name
                "CoolAbout", // URL with parameters
                new { controller = "Home", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void Start2() {
            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Bar2", // Route name
                "CoolAbout2", // URL with parameters
                new { controller = "Home", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void CallMeAfterAppStart() {
            // This gets called after global.asax's Application_Start
        }
    }
}
