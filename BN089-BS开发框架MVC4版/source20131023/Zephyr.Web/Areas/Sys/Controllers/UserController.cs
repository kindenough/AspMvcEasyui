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
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class UserApiController : ApiController
    {
        public dynamic Get(RequestWrapper request) 
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='UserSeq'>
   <where>
        <field name='UserCode' cp='mapchild' extend='sys_userOrganizeMap,OrganizeCode,sys_organize' variable='OrganizeCode' ignoreEmpty='true'></field>
    </where>
</settings>");
            var service = new sys_userService();
            var result = service.GetModelListWithPaging(request.ToParamQuery());
            return result;
        }

        public dynamic GetSettingList(string id)
        {
            var pQuery = ParamQuery.Instance().AndWhere("UserCode", id);
            var service = new sys_userSettingService();
            return service.GetModelList(pQuery);
        }

        public dynamic GetOrganizeWithUserCheck(string id)
        {
            var service = new sys_userService();
            return service.GetUserOrganize(id);
        }

        public dynamic GetRoleWithUserCheck(string id)
        {
            var service = new sys_userService();
            return service.GetUserRole(id);
        }

        [System.Web.Http.HttpPost]
        public int PostResetPassword(string id)
        {
            var service = new sys_userService();
            return service.ResetUserPassword(id);
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_user</table>
    <where>
        <field name='UserCode' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_userService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void EditUserOrganizes(string id, dynamic data)
        {
            var service = new sys_userService();
            service.SaveUserOrganizes(id, data as JToken);
        }

        [System.Web.Http.HttpPost]
        public void EditUserRoles(string id, dynamic data)
        {
            var service = new sys_userService();
            service.SaveUserRoles(id, data as JToken);
        }

        [System.Web.Http.HttpPost]
        public void EditUserSetting(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_userSetting</table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_userSettingService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
