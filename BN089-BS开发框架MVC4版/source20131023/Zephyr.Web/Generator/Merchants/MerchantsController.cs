
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
    public class MerchantsController : Controller
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
                    query = "/api/mms/Merchants",
                    newkey = "/api/mms/Merchants/getnewkey",
                    edit = "/api/mms/Merchants/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    MerchantsTypeName = "" ,
                    RegisterFund = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "MerchantsCode",
                    postListFields = new string[] { "ChargePerson" ,"BuildDate" }
                };
            };

            return View(model);
        }

    }

    public class MerchantsApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>mms_merchants</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='MerchantsTypeName' cp='equal'></field>   
        <field name='RegisterFund' cp='equal'></field>   
    </where>
</settings>");
            var service = new mms_merchantsService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_merchantsService().GetNewKey("MerchantsCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_merchants
    </table>
    <where>
        <field name='MerchantsCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_merchantsService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
