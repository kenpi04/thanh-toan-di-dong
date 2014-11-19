using System.Web.Routing;
using PlanX.Web.Framework.Localization;
using PlanX.Web.Framework.Mvc.Routes;
using PlanX.Web.Framework.Seo;

namespace PlanX.Web.Infrastructure
{
    public partial class GenericUrlRouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //generic URLs
            routes.MapGenericPathRoute("GenericUrl",
                                       "{generic_se_name}",
                                       new {controller = "Common", action = "GenericUrl"},
                                       new[] {"PlanX.Web.Controllers"});

            //define this routes to use in UI views (in case if you want to customize some of them later)
            routes.MapLocalizedRoute("Product",
                                     "{SeName}",
                                     new {controller = "Catalog", action = "Product"},
                                     new[] {"PlanX.Web.Controllers"});

            routes.MapLocalizedRoute("Category",
                            "{SeName}",
                            new { controller = "Catalog", action = "Category" },
                            new[] { "PlanX.Web.Controllers" });

            routes.MapLocalizedRoute("Manufacturer",
                            "{SeName}",
                            new { controller = "Catalog", action = "Manufacturer" },
                            new[] { "PlanX.Web.Controllers" });

            routes.MapLocalizedRoute("NewsItem",
                            "{SeName}",
                            new { controller = "News", action = "NewsItem" },
                            new[] { "PlanX.Web.Controllers" });

            routes.MapLocalizedRoute("BlogPost",
                            "{SeName}",
                            new { controller = "Blog", action = "BlogPost" },
                            new[] { "PlanX.Web.Controllers" });



            //the last route. it's used when none of registered routes could be used for the current request
            //but it this case we cannot process non-registered routes (/controller/action)
            //routes.MapLocalizedRoute(
            //    "PageNotFound-Wildchar",
            //    "{*url}",
            //    new { controller = "Common", action = "PageNotFound" },
            //    new[] { "PlanX.Web.Controllers" });
        }

        public int Priority
        {
            get
            {
                //it should be the last route
                //we do not set it to -int.MaxValue so it could be overriden (if required)
                return -1000000;
            }
        }
    }
}
