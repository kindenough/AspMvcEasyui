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
    public class PermissionController : Controller
    {
        //
        // GET: /Sys/AuthCode/

        public ActionResult Index()
        {
            return View();
        }

    }

    public class PermissionApiController : ApiController
    {
        // GET api/permission
        public IEnumerable<dynamic> Get()
        {
            var pQuery = ParamQuery.Instance().Select("A.*,B.PermissionName as ParentName")
                .From(@"sys_permission A left join sys_permission B on B.PermissionCode = A.ParentCode");
            var result = new sys_permissionService().GetDynamicList(pQuery);
            return result;
        }

        public IEnumerable<dynamic> GetRolePermission(string id)
        {
            var pQuery = ParamQuery.Instance().Select(@"A.*
,case when B.PermissionCode is null then 0 else 1           end as checked
,case when B.PermissionCode is null then 0 else B.IsDefault end as IsDefault")
                .From(String.Format(@"sys_permission A
left join sys_rolePermissionMap B on B.RoleCode='{0}' and B.PermissionCode = A.PermissionCode ",id));
            var result = new sys_permissionService().GetDynamicList(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_permission
    </table>
    <where>
        <field name='PermissionCode' cp='equal' variable='_id'></field>
    </where>
</settings>");

            var service = new sys_permissionService();
            var result = service.Edit(null, listWrapper, data);

            //service.Logger("api/mms/send", "菜单数据", "修改", data);
        }
    }

}
