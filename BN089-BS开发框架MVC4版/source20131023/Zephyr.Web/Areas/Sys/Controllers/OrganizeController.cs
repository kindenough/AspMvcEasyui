using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class OrganizeController : Controller
    {
        public ActionResult Index()
        {
            var model = new sys_organizeService().GetModelList();
            return View(model);
        }
    }

    public class OrganizeApiController : ApiController
    {
        public dynamic Get() 
        {
            return new sys_organizeService().GetModelList();
        }

        public dynamic GetRoleWithOrganizeCheck(string id)
        {
            var service = new sys_organizeService();
            return service.GetOrganizeRole(id);
        }

        [System.Web.Http.HttpPost]
        public dynamic Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_organize
    </table>
    <where>
        <field name='OrganizeCode' cp='equal' variable='_OrganizeCode'></field>
    </where>
</settings>");
            var service = new sys_organizeService();
            service.Edit(formWrapper, null, data);

            var result = service.GetModelList();
            return result;
        }

        public dynamic Delete(string id)
        {
            var service = new sys_organizeService();
            service.RecursionDelete(id);
            var result = service.GetModelList();
            return result;
        }

        [System.Web.Http.HttpPost]
        public void EditOrganizeRoles(string id, dynamic data)
        {
            var service = new sys_organizeService();
            service.SaveOrganizeRoles(id, data as JToken);
        }
    }
}
