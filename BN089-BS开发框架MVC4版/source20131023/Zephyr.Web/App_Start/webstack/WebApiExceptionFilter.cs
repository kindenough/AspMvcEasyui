using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using log4net;

namespace Zephyr.Web
{
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ILog log = LogManager.GetLogger(HttpContext.Current.Request.Url.LocalPath);
            log.Error(context.Exception);

            var message = context.Exception.Message;
            if (context.Exception.InnerException != null) 
                message = context.Exception.InnerException.Message;

            context.Response = new HttpResponseMessage() { Content = new StringContent(message) };

            base.OnException(context);
        }
    }
}