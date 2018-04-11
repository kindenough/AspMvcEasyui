
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
    public class MenuController : Controller
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
                    query = "/api/mms/Menu",
                    newkey = "/api/mms/Menu/getnewkey",
                    edit = "/api/mms/Menu/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    MenuCode = "" ,
                    MenuName = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = MenuCode;
                    postListFields = new string[] { "MenuCode" ,"MenuName" ,"URL" ,"IconClass" ,"IconURL" }
                };

           return View(model);
        }

    }

    public class MenuApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>sys_menu</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='MenuCode' cp='like'></field>   
        <field name='MenuName' cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_menuService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new sys_menuService().GetNewKey("MenuCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_menu
    </table>
    <where>
        <field name='MenuCode' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_menuService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
