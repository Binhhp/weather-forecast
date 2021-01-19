using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebScrapper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Weather New",
               url: "weather/news",
               defaults: new { controller = "New", action = "News", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Web Scrapper",
               url: "weather/web-scrapper",
               defaults: new { controller = "Scrapper", action = "webScrapper", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Web API",
               url: "weather/web-api",
               defaults: new { controller = "WebAPI", action = "WeatherAPI", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "WebAPI", action = "WeatherAPI", id = UrlParameter.Optional }
            );
        }
    }
}
