using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Zephyr.Web
{
    public class JObjectModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var stream = controllerContext.RequestContext.HttpContext.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(stream).ReadToEnd();
            return JsonConvert.DeserializeObject<dynamic>(json);
        }
    }
}