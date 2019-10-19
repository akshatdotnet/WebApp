﻿using System.Web.Mvc;
using System.Web.Routing;

namespace MVCTutorial
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "chosen", action = "AddOrEdit", id = UrlParameter.Optional }
                defaults: new { controller = "Test", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "Repo", action = "Index", id = UrlParameter.Optional }chosen
            );
        }
    }
}
