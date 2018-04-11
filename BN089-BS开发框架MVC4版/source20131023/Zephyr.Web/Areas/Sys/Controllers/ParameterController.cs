using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class ParameterController : Controller
    {
        //
        // GET: /Sys/Parameter/

        public ActionResult Index()
        {
            return View();
        }

    }

    public class ParameterApiController : ApiController
    {
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString("<settings defaultOrderBy='ParamCode'></settings>");
            return new sys_parameterService().GetModelListWithPaging(request.ToParamQuery());
        }
 
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_parameter
    </table>
    <where>
        <field name='ParamCode' cp='equal' variable='_id'></field>
    </where>
</settings>");
            var service = new sys_parameterService();
            var result = service.Edit(null, listWrapper, data);
        }

    }
}
