
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
    public class StaffController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Mms/Staff/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Staff/edit/",                      //数据保存api
                    audit = "/api/Mms/Staff/audit/",                    //审核api
                    newkey = "/api/Mms/Staff/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new StaffApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new REMP_Staff().Extend(new {  EMPSID = "",DEPTID = "",UserName = "",EmployeeName = "",DisplayName = "",RealNameSpell = "",UserType = "",EmployeeCode = "",Gender = "",Birthday = "",Nationality = ""}),
                    primaryKeys = new string[]{"EMPSID"}
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      defaults = new {CHANID = "",EMPSID = "",changedate = "",changetype = "",createtime = ""},
                      postFields = new string[] { "CHANID","EMPSID","changedate","changetype","createtime"}
                    },    
                    new{
                      type = "form",
                      defaults = new {Folk = "",Native = "",VoucherName = "",UserIndentity = "",Register = "",OfficeAddress = ""},
                      primaryKeys = new string[]{"EMPSID"}
                    },    
                    new{
                      type = "form",
                      defaults = new {HomeAddress = "",HomeZipCode = "",OfficeFax = "",IsStaff = "",InDutyDate = "",TryDutyEndDate = "",Duty = "",OutDuty = "",DutyLevel = ""},
                      primaryKeys = new string[]{"EMPSID"}
                    }    
                }
            };
            return View(model);
        }
    }

    public class StaffApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new REMP_StaffService();
            var pQuery = ParamQuery.Instance().AndWhere("EMPSID", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("EMPSID", id),

                //明细数据
                tab0 = new REMP_ChangeService().GetDynamicList(ParamQuery.Instance().AndWhere("CHANID", id)),
                tab1 = new REMP_StaffService().GetModel(pQuery),      
                tab2 = new REMP_StaffService().GetModel(pQuery)      
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("REMP_Staff")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("EMPSID", id);

            var service = new REMP_StaffService();
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
                    var service0 = new REMP_ChangeService();
                    return service0.GetNewKey("CHANID", "maxplus", qty, ParamQuery.Instance().AndWhere("CHANID", key, Cp.Equal));
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
        REMP_Staff
    </table>
    <where>
        <field name='EMPSID' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>REMP_Change</table>
    <where>
        <field name='CHANID' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>REMP_Staff</table>
    <where>
        <field name='EMPSID' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>REMP_Staff</table>
    <where>
        <field name='EMPSID' cp='equal'></field>
    </where>
</settings>"));
             
            var service = new REMP_StaffService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
