using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace EReceipt.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
               name: "DefaultApi1",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { action = "get", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "ApiPostById",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { action = "post", id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            jsonFormatter.SerializerSettings.Culture = new CultureInfo(string.Empty)
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalDigits = 5
                }
            };

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}
