using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sankyo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Login",
               url: "login",
               defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Register",
               url: "register",
               defaults: new { controller = "Home", action = "Register", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ListTopic",
               url: "list-topic",
               defaults: new { controller = "Home", action = "ListTopic", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "AddTopic",
               url: "add-topic",
               defaults: new { controller = "Home", action = "AddTopic", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "EditTopic",
               url: "edit-topic/{id}",
               defaults: new { controller = "Home", action = "AddTopic", id = @"\d+" }
           );
            routes.MapRoute(
               name: "TopicDetail",
               url: "{sename}-{id}",
               defaults: new { controller = "Home", action = "Index", sename = UrlParameter.Optional, id = UrlParameter.Optional }
               
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

   
}