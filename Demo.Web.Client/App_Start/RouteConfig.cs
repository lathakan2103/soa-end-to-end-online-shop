using System.Web.Mvc;
using System.Web.Routing;

namespace Demo.Web.Client
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "accountRegisterRoot",
                "account/register",
                new { controller = "Account", action = "Register"},
                new[] { "Demo.Web.Controllers.Mvc" }
            );

            routes.MapRoute(
                "accountRegister",
                "account/register/{*catchall}",
                new { controller = "Account", action = "Register" },
                new[] { "Demo.Web.Controllers.Mvc" }
            );

            routes.MapRoute(
                "buyRoot",
                "customer/buy",
                new { controller = "Customer", action = "Buy" },
                new[] { "Demo.Web.Controllers.Mvc" }
            );

            routes.MapRoute(
                "buyList",
                "customer/buy/{*catchall}",
                new { controller = "Customer", action = "Buy" },
                new[] { "Demo.Web.Controllers.Mvc" }
            );

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Demo.Web.Controllers.Mvc" }
            );
        }
    }
}
