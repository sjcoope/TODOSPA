using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SJCNet.Todo.Web.App_Start.WebApiConfig), "Register")]

namespace SJCNet.Todo.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            Configure(config);

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{action}",
                defaults: new { Controller = "Default" }
            );

            config.Routes.MapHttpRoute(
                name: "Module",
                routeTemplate: "api/{controller}/{action}"
            );
        }

        private static void Configure(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Formatters.Add(json);
        }
    }
}