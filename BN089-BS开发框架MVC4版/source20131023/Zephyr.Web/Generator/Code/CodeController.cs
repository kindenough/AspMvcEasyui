
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
    public class CodeController : Controller
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
                    query = "/api/mms/Code",
                    newkey = "/api/mms/Code/getnewkey",
                    edit = "/api/mms/Code/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    Code = "" ,
                    Text = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    postListFields = new string[] { "Value" ,"Text" ,"Seq" }
                };

           return View(model);
        }

    }

    public class CodeApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>sys_code</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Code' cp='equal'></field>   
        <field name='Text' cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_codeService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new sys_codeService().GetNewKey("Code", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_code
    </table>
    <where>
        <field name='Code' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_codeService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
