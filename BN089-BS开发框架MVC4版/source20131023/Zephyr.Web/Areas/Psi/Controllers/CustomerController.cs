using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Psi.Controllers
{
    public class CustomerController : Controller
    {
        #region 查询页面
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new{
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new{
                    query = "/api/psi/customer",
                    remove = "/api/psi/customer/",
                    billno = "/api/psi/customer/getnewbillno",
                    audit = "/api/psi/customer/audit/",
                    edit = "/psi/customer/edit/"
                },
                resx = new{
                    detailTitle = "客户维护",
                    noneSelect = "请先选择一个客户！",
                    deleteConfirm = "确定要删除选中的客户吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    Id = "",
                    ReceivePerson = "",
                    SupplierName = "",
                    Warehouse = "",
                    Contract="",
                    ReceiveDate = ""
                }
            };

            return View(model);
        }
        #endregion

        #region 编辑页面
        public ActionResult Edit(string id = "")
        {
            var data = new CustomerApiController().GetEditMaster(id);
            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                dataSource = new
                {
                    //warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    getdetail = "/api/psi/customer/getdetail/",
                    getmaster = "/api/psi/customer/geteditmaster/",
                    edit = "/api/psi/customer/edit",
                    audit = "/api/psi/customer/audit/",
                    getrowid = "/api/psi/customer/getnewrowid/",
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                defaultRow = new
                {
                    ContractAmount=0,
                    PrePay = 0,
                    Payed=0,
                    NoPay=0,
                    BeginDate = DateTime.Now,
                    Deadline =0
                },
                setting = new
                {
                    postFormKeys = new string[] { "Id" },
                    postListFields = new string[] { "Id" }
                }
            };

            return View(model);
        }
        #endregion

        #region 报销页面
        public ActionResult Claim()
        {
            var model = new
            {
                dataSource = new
                {
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    query = "/api/psi/goods",
                    newkey = "/api/psi/goods/getnewkey",
                    edit = "/api/psi/goods/edit"//audit = "/api/psi/audit";
                },
                resx = new
                {
                    noneSelect = "请先选择一条货物数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    Id = "",
                    Name = "",
                    Catagory = "",
                    Brand = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    postListFields = new string[] { "Id", "Name", "Catagory", "Brand", "Model", "Unit", "Remark" }
                }
            };

            return View(model);
        }
        #endregion
    }

    public class CustomerApiController : ApiController
    {
        #region 查询页面 api
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='Id'>
    <select>
        A.*
    </select>
    <from>
        psi_customer A
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Name'          cp='like'       ></field>
        <field name='ChargePerson'  cp='like'       ></field>
        <field name='Catagory'      cp='like'></field>
        <field name='MainService'   cp='like' ></field>
    </where>
</settings>");  

            var ReceiveService = new psi_customerService();
            var pQuery = query.ToParamQuery();
            var result = ReceiveService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewId()
        {
            var service = new psi_customerService();
            return service.GetNewKey("Id", "dateplus");
        }

        public void Delete(string id)
        {
            var service = new psi_customerService();
            var result = service.Delete(ParamDelete.Instance().AndWhere("Id", id));
            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("收货单删除失败{Id=" + id + "}，请重试或联系管理员！") });
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, dynamic data)
        {
            var service = new psi_customerService();
            var result = service.Update(ParamUpdate.Instance()
                .Column("AuditState", data.status)
                .Column("AuditReason", data.comment)
                .Column("AuditPerson", FormsAuth.GetUserData().UserName)
                .Column("AuditDate", DateTime.Now)
                .AndWhere("Id", id));

            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("审核收货单失败{Id=" + id + "}，请重试或联系管理员！") });
        }
        #endregion 

        #region 编辑页面用 api
        public dynamic GetEditMaster(string id) {
            dynamic result = new ExpandoObject();
            var masterService = new psi_customerService();
            result.form = masterService.GetModel(ParamQuery.Instance().AndWhere("Id", id));
            result.form.Id = id; //如果没有数据，返回一条空数据，Id=id; 对应新增的情况
            result.scrollKeys = masterService.ScrollKeys("Id", id);
            return result;
        }
 
        public string GetNewRowId(int id)
        {
            var service = new psi_customerContractService();
            return service.GetNewKey("ContractNo", "maxplus", id);
        }

        public dynamic GetDetail(string id)
        {
            var ReceiveService = new psi_customerService();
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("CustomerId",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='UpdateDate desc'>
    <select>
        A.*
    </select>
    <from>
        psi_customerContract A
    </from>
    <where>
        <field name='CustomerId' cp='equal'></field>
    </where>
</settings>");

            var pQuery1 = query.ToParamQuery();
            
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='VisitId desc'>
    <select>
        A.*
    </select>
    <from>
        psi_customerVisit A
    </from>
    <where>
        <field name='CustomerId' cp='equal'></field>
    </where>
</settings>");

            var pQuery2 = query.ToParamQuery();

            //var result = new {
            //    contract: ReceiveService.GetDynamicListWithPaging(pQuery1),
            //    visit:ReceiveService.GetDynamicListWithPaging(pQuery2)
            //};
            var result = ReceiveService.GetDynamicListWithPaging(pQuery1);
            return result;
        }
 
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_customer
    </table>
    <where>
        <field name='Id' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_customerContract
    </table>
    <where>
        <field name='ContractNo' cp='equal'></field>
    </where>
</settings>");
             
            var service = new psi_customerService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}