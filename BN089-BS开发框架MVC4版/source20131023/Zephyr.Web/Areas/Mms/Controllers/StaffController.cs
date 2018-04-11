
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class StaffController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/Staff",
                    remove = "/api/Mms/Staff/",
                    billno = "/api/Mms/Staff/getnewbillno",
                    audit = "/api/Mms/Staff/audit/",
                    edit = "/Mms/Staff/edit/"
                },
                resx = new
                {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    dsUserType = new sys_codeService().GetValueTextListByType("PersonType")
                },
                form = new
                {
                    UserName = "",
                    EmployeeName = "",
                    DisplayName = "",
                    UserType = "",
                    EmployeeCode = "",
                    Birthday = ""
                },
                idField = "EMPSID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/Staff/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Staff/edit/",                      //数据保存api
                    audit = "/api/Mms/Staff/audit/",                    //审核api
                    newkey = "/api/Mms/Staff/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed = "单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new
                {
                    pageData = new StaffApiController().GetPageData(id),
                    dsUserType = new sys_codeService().GetValueTextListByType("PersonType"),
                    dsDep = new REMP_DepartmentService().GetDynamicList(ParamQuery.Instance().Select("deptname as text,DEPTID as value"))
                },
                form = new
                {
                    defaults = new REMP_Staff().Extend(new { EMPSID = "", DEPTID = "", UserName = "", EmployeeName = "", DisplayName = "", RealNameSpell = "", UserType = "", EmployeeCode = "", Gender = "", Birthday = "", Nationality = "" }),
                    primaryKeys = new string[] { "EMPSID" }
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      rowId = "CHANID",
                      relationId = "EMPSID",
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

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='EMPSID'>
    <select>*</select>
    <from>REMP_Staff</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserName'		cp='equal'></field>   
        <field name='EmployeeName'		cp='equal'></field>   
        <field name='DisplayName'		cp='equal'></field>   
        <field name='UserType'		cp='equal'></field>   
        <field name='EmployeeCode'		cp='equal'></field>   
        <field name='Birthday'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new REMP_StaffService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetPageData(string id)
        {
            var masterService = new REMP_StaffService();
            var pQuery = ParamQuery.Instance().AndWhere("EMPSID", id);

            var result = new
            {
                //主表数据
                form = masterService.GetDynamic(pQuery),
                scrollKeys = masterService.ScrollKeys("EMPSID", id),

                //明细数据
                tab0 = new REMP_ChangeService().GetDynamicList(ParamQuery.Instance().AndWhere("EMPSID", id)),
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
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new REMP_ChangeService();
                    return service0.GetNewKey("CHANID", "maxplus", qty, ParamQuery.Instance().AndWhere("EMPSID", key, Cp.Equal));
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
