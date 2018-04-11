
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
    public class LossReportController : Controller
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
                    query = "/api/mms/LossReport",
                    newkey = "/api/mms/LossReport/getnewkey",
                    edit = "/api/mms/LossReport/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    BillNo = "" ,
                    BillDate = "" ,
                    DoPerson = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    postListFields = new string[] { "BillNo" ,"BillDate" ,"DoPerson" ,"ProjectCode" }
                };

           return View(model);
        }

    }

    public class LossReportApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>mms_lossReport</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo' cp='equal'></field>   
        <field name='BillDate' cp='equal'></field>   
        <field name='DoPerson' cp='equal'></field>   
    </where>
</settings>");
            var service = new mms_lossReportService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_lossReportService().GetNewKey("BillNo", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_lossReport
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_lossReportService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
