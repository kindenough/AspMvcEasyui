
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
    public class DealController : Controller
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
                    query = "/api/Mms/Deal",
                    newkey = "/api/Mms/Deal/getnewkey",
                    edit = "/api/Mms/Deal/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    ProjectCode = "" ,
                    ApplyDate = "" ,
                    DealType = "" ,
                    DealKind = "" ,
                    TotalMoney = "" ,
                    ApproveState = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "BillNo",
                    postListFields = new string[] { "BillNo" ,"BillDate" ,"ProjectCode" ,"DoPerson" ,"ApplyDate" ,"DealType" ,"DealKind" ,"TotalMoney" ,"ApproveState" ,"ApprovePerson" }
                }
            };

            return View(model);
        }
    }

    public class DealApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>*</select>
    <from>mms_deal</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ProjectCode'		cp='like'></field>   
        <field name='ApplyDate'		cp='daterange'></field>   
        <field name='DealType'		cp='equal'></field>   
        <field name='DealKind'		cp='equal'></field>   
        <field name='TotalMoney'		cp='equal'></field>   
        <field name='ApproveState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new mms_dealService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_dealService().GetNewKey("BillNo", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_deal
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_dealService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
