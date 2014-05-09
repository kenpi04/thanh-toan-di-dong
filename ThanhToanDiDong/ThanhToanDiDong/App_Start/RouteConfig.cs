using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThanhToanDiDong
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          
            routes.MapRoute(
             name: "HomePage",
             url: "",
             defaults: new { controller = "Home", action = "Index" },
             namespaces: new[] { "ThanhToanDiDong.Controllers" }

         );
            routes.MapRoute(
            name: "Success",
            url: "giao-dich-thanh-cong",
            defaults: new { controller = "Payment", action = "PaymentSuccess" },
            namespaces: new[] { "ThanhToanDiDong.Controllers" }

        );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "ThanhToanDiDong.Controllers" }

          );
        }
    }
}