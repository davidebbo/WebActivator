using System.Web.Mvc;
using System.Web.Routing;
using WebActivation;

namespace TestLibrary {
    public class MyStartupCode2 : IApplicationStart {
        public void Start() {
            var routes = RouteTable.Routes;

            routes.MapRoute(
                "Bar", // Route name
                "CoolAbout", // URL with parameters
                new { controller = "Home", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
