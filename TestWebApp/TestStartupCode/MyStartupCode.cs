using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestWebApp.TestStartupCode.MyStartupCode), "Start")]

namespace TestWebApp.TestStartupCode {
    public static class MyStartupCode {
        public static void Start() {
            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Foo", // Route name
                "CoolHome", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
