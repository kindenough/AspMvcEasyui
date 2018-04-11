
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class LossReportBatchesController : Controller
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
                    query = "/api/Mms/LossReportBatches",
                    newkey = "/api/Mms/LossReportBatches/getnewkey",
                    edit = "/api/Mms/LossReportBatches/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    MaterialCode = "" ,
                    Money = "" ,
                    SrcRowId = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "RowId",
                    postListFields = new string[] { "RowId" ,"Num" ,"SrcBillNo" }
                }
            };

            return View(model);
        }
    }

    public class LossReportBatchesApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>*</select>
    <from>mms_lossReportBatches</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='MaterialCode'		cp='equal'></field>   
        <field name='Money'		cp='equal'></field>   
        <field name='SrcRowId'		cp='equal'></field>   
    </where>
</settings>");
            var service = new mms_lossReportBatchesService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_lossReportBatchesService().GetNewKey("RowId", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_lossReportBatches
    </table>
    <where>
        <field name='RowId' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_lossReportBatchesService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
