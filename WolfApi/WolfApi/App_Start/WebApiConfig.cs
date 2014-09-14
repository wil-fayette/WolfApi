using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WolfApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
               config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });
            config.EnableCors();
        }
    }
}
