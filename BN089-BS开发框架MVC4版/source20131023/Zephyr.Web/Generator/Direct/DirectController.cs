
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class DirectController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Mms/Direct/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Direct/edit/",                      //数据保存api
                    audit = "/api/Mms/Direct/audit/",                    //审核api
                    newkey = "/api/Mms/Direct/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new DirectApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new mms_direct().Extend(new {  BillNo = "",BillDate = "",DoPerson = "",SupplierCode = "",ContractCode = "",ArriveDate = "",BuildPartCode = "",PayKind = "",OriginalNum = "",ApprovePerson = "",TotalMoney = "",ApproveState = ""}),
                    primaryKeys = new string[]{"BillNo"}
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      defaults = new {BillNo = "",RowId = "",MaterialCode = "",Unit = "",UnitPrice = "",Num = "",Money = "",CreatePerson = "",CreateDate = "",UpdatePerson = "",UpdateDate = "",Remark = ""},
                      postFields = new string[] { "BillNo","RowId","MaterialCode","Unit","UnitPrice","Num","Money","CreatePerson","CreateDate","UpdatePerson","UpdateDate","Remark"}
                    },    
                    new{
                      type = "form",
                      defaults = new {ApproveRemark = "",CreatePerson = "",CreateDate = "",UpdatePerson = "",UpdateDate = "",PayKind = "",PickPerson = "",BuildPartCode = ""},
                      primaryKeys = new string[]{"BillNo"}
                    },    
                    new{
                      type = "grid",
                      defaults = new {UseUnit = "",Num = "",UnitPrice = ""},
                      postFields = new string[] { "UseUnit","Num","UnitPrice"}
                    },    
                    new{
                      type = "empty",
                      defaults = new {ApproveState = "",ApproveRemark = "",ApprovePerson = "",ApproveDate = "",CreatePerson = "",CreateDate = "",UpdatePerson = "",UpdateDate = ""},
                    },    
                    new{
                      type = "form",
                      defaults = new {ChargePerson = "",RegisterFund = ""},
                      primaryKeys = new string[]{"MerchantsCode"}
                    }    
                }
            };
            return View(model);
        }
    }

    public class DirectApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new mms_directService();
            var pQuery = ParamQuery.Instance().AndWhere("BillNo", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("BillNo", id),

                //明细数据
                tab0 = new mms_directDetailService().GetDynamicList(pQuery),
                tab1 = new mms_directService().GetModel(pQuery),      
                tab2 = new mms_rentInDetailService().GetDynamicList(ParamQuery.Instance().AndWhere("RowId", id)),
                tab4 = new mms_merchantsService().GetModel(ParamQuery.Instance().AndWhere("MerchantsCode", id))      
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("mms_direct")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("BillNo", id);

            var service = new mms_directService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
  
        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type,string key,int qty=1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new mms_directDetailService();
                    return service0.GetNewKey("RowId", "maxplus", qty, ParamQuery.Instance().AndWhere("BillNo", key, Cp.Equal));
                case "grid2":
                    var service2 = new mms_rentInDetailService();
                    return service2.GetNewKey("BillNo", "maxplus", qty, ParamQuery.Instance().AndWhere("RowId", key, Cp.Equal));
                default:
                    return "";
            }
        }
  
        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_direct
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_directDetail</table>
    <where>
        <field name='BillNo' cp='equal'></field>      
        <field name='RowId' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_direct</table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_rentInDetail</table>
    <where>
        <field name='BillNo' cp='equal'></field>      
        <field name='RowId' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_merchants</table>
    <where>
        <field name='MerchantsCode' cp='equal'></field>
    </where>
</settings>"));
             
            var service = new mms_directService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
