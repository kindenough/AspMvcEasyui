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
    public class RoleController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                users = new sys_userService().GetDynamicList(ParamQuery.Instance().Select("UserCode,UserName")),
                organizes = new sys_organizeService().GetDynamicList(ParamQuery.Instance().Select("OrganizeCode,OrganizeName"))
            };
            return View(model);
        }
    }

    public class RoleApiController : ApiController
    {
        public dynamic GetRoleMenu(string id) 
        {
            var service = new sys_roleService();
            var pQuery = ParamQuery.Instance().Select(String.Format(@" A.MenuCode,
        (case when exists(select 1 from sys_roleMenuMap B where B.MenuCode = A.MenuCode and B.RoleCode = '{0}')
              then '1' 
              else '0' end) as chk
            ", id)).From("sys_menu A")
             .AndWhere("A.IsEnable",true);

            var result = service.GetDynamicList(pQuery);
            return result;
        }

        public dynamic GetRoleMembers(string id) 
        {
            var result = new sys_userService().GetRoleMembers(id);
            return result;
        }
 
        public IEnumerable<dynamic> Get(RequestWrapper req)
        {
            req.LoadSettingXmlString("<settings defaultOrderBy='RoleSeq'></settings>");
            var result = new sys_roleService().GetModelList(req.ToParamQuery());
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_role
    </table>
    <where>
        <field name='RoleCode' cp='equal' variable='_id'></field>
    </where>
</settings>");

            var service = new sys_roleService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void EditPermission(string id, dynamic data)
        {
            var service = new sys_roleService();
            service.EditRoleMenu(id, data.menus as JToken);
            service.EditRoleMenuButton(id, data.buttons as JToken);
            service.EditRolePermission(id, data.permissions as JToken);
            service.EditRoleMenuColumns(id, data.columns as JToken);
        }

        [System.Web.Http.HttpPost]
        public void EditRoleMembers(string id, dynamic data)
        {
            var service = new sys_roleService();
            service.SaveRoleMembers(id, data as JToken);
        }
    }
}
