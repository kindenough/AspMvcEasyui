using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Areas.Mms.Controllers
{
    public class WarehouseController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }
    }

    public class WarehouseApiController : ApiController
    {
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='WarehouseCode'>
   <where>
        <field name='ProjectCode' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var service = new mms_warehouseService();
            var result = service.GetModelListWithPaging(request.ToParamQuery());
            return result;
        }

        public string GetNewCode(string id)
        {
            var service = new mms_warehouseService();
            return service.GetNewKey("WarehouseCode", "maxplus").PadLeft(3, '0');
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_warehouse</table>
    <where>
        <field name='WarehouseCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_warehouseService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
