using System.IO;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.XPath;
using ChuangLiTouOpenApi.Factory;
using Newtonsoft.Json.Converters;

namespace ChuangLiTouOpenApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //规则
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
                );

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new IsoDateTimeConverter
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                }
                );

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
            //返回格式选择 datatype 可以替换为任何参数 
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "xml", "application/xml"));
        }
    }
}
