using System.Net.Http.Headers;
using System.Web.Http;

namespace DocumentGenerator.WebAPI.Startup
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
      config.MapHttpAttributeRoutes();
    }
  }
}