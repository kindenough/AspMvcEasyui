
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class LogController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = new{
                    query = "/api/mms/Log",
                    newkey = "/api/mms/Log/getnewkey",
                    edit = "/api/mms/Log/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    UserCode = "" ,
                    UserName = "" ,
                    Target = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "ID",
                    postListFields = new string[] { "UserCode" ,"UserName" ,"Position" ,"Target" }
                };
            };

            return View(model);
        }

    }

    public class LogApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>sys_log</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserCode' cp='equal'></field>   
        <field name='UserName' cp='equal'></field>   
        <field name='Target' cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_logService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new sys_logService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_log
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_logService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
