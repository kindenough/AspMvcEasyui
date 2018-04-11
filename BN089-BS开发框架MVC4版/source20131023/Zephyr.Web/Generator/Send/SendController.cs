
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
    public class SendController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Mms/Send/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Send/edit/",                      //数据保存api
                    audit = "/api/Mms/Send/audit/",                    //审核api
                    newkey = "/api/Mms/Send/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new SendApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new mms_send().Extend(new {  BillNo = "",DoPerson = "",MaterialType = "",ProjectCode = "",WarehouseCode = "",SendDate = "",Purpose = "",PickUnit = ""}),
                    primaryKeys = new string[]{"BillNo"}
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      rowId = "RowId",
                      relationId = "BillNo",
                      postFields = new string[] { "BillNo","RowId","MaterialCode","Num","Money","SrcBillNo"},
                      defaults = new {BillNo = "",RowId = "",MaterialCode = "",Num = "",Money = "",SrcBillNo = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"BillNo"},
                      defaults = new {ApproveState = "",ApproveRemark = "",ApprovePerson = "",ApproveDate = "",CreatePerson = "",CreateDate = "",UpdatePerson = "",UpdateDate = ""}
                    },    
                    new{
                      type = "empty",
                      defaults = new {ApproveState = "",ApproveRemark = "",ApprovePerson = "",ApproveDate = "",CreatePerson = "",CreateDate = "",UpdatePerson = "",UpdateDate = ""}
                    }    
                }
            };
            return View(model);
        }
    }

    public class SendApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new mms_sendService();
            var pQuery = ParamQuery.Instance().AndWhere("BillNo", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("BillNo", id),

                //明细数据
                tab0 = new mms_sendBatchesService().GetDynamicList(pQuery),
                tab1 = new mms_transferService().GetModel(pQuery),      
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("mms_send")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("BillNo", id);

            var service = new mms_sendService();
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
                    var service0 = new mms_sendBatchesService();
                    return service0.GetNewKey("RowId", "maxplus", qty, ParamQuery.Instance().AndWhere("BillNo", key, Cp.Equal));
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
        mms_send
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_sendBatches</table>
    <where>
        <field name='BillNo' cp='equal'></field>      
        <field name='RowId' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_transfer</table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>"));
             
            var service = new mms_sendService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
