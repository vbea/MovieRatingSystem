using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieRatingSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Category",
                url: "Category/{action}",
                defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "User",
                url: "User/{action}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "UserInfo",
                url: "UserInfo/{id}",
                defaults: new { controller = "UserInfo", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Movie",
                url: "Movie/{action}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Comment",
                url: "Comment/{action}",
                defaults: new { controller = "Comment", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Poster",
                url: "Poster/{action}",
                defaults: new { controller = "Poster", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default1",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
