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
    public class PurchaseController : Controller
    {
        #region 查询页面
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    //warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    query = "/api/psi/purchase",
                    remove = "/api/psi/purchase/",
                    billno = "/api/psi/purchase/getnewbillno",
                    audit = "/api/psi/purchase/audit/",
                    edit = "/psi/purchase/edit/"
                },
                resx = new
                {
                    detailTitle = "采购单明细",
                    noneSelect = "请先选择一条采购单！",
                    deleteConfirm = "确定要删除选中的采购单吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    BillNo = "",
                    PurchasePerson = "",
                    SupplierName = "",
                    IsEffect = "true",
                    Contract = "",
                    PurchaseDate = ""
                }
            };

            return View(model);
        }
        #endregion

        #region 编辑页面
        public ActionResult Edit(string id = "")
        {
            var userName = FormsAuth.GetUserData().UserName;
            var data = new PurchaseApiController().GetEditMaster(id);
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
                    getdetail = "/api/psi/purchase/getdetail/",
                    getmaster = "/api/psi/purchase/geteditmaster/",
                    edit = "/api/psi/purchase/edit",
                    audit = "/api/psi/purchase/audit/",
                    getrowid = "/api/psi/purchase/getnewrowid/",
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                defaultForm=new{
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    DoDate = DateTime.Now,
                    Supplier = "",
                    ContractName = "",
                    PurchaseDate = DateTime.Now,
                    PurchasePerson = userName,
                    TotalMoney = 0,
                    Remark = "",
                    AuditPerson = userName,
                    AuditDate = DateTime.Now,
                    AuditState = "Unapproved",
                    AuditReason = "",
                    CreatePerson = userName,
                    CreateDate = DateTime.Now,
                    UpdatePerson = "",
                    UpdateDate = ""
                },
                defaultRow = new
                {
                    Num = 1,
                    UnitPrice = 0,
                    Money = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "GoodNo", "Num", "UnitPrice", "Money", "Remark" }
                }
            };

            return View(model);
        }
        #endregion
    }

    public class PurchaseApiController : ApiController
    {
        #region 查询页面 api
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*,B.Name as SupplierName,case when A.AuditState='passed' then 'true' else 'false' end as IsEffect
    </select>
    <from>
        psi_purchase A
        left join psi_supplier  B on B.Id = A.Supplier
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'                cp='equal'      ></field>
        <field name='PurchasePerson'        cp='like'      ></field>
        <field name='Contract'              cp='like'       ></field>
        <field name='PurchaseDate'          cp='daterange'    ></field>
        <field name='C.Name'                cp='like' variable='SupplierName'></field>
    </where>
</settings>");

            var PurchaseService = new psi_purchaseService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("AuditState", query["IsEffect"], x => x.Value == "true"?"AuditState='passed'":"isnull(AuditState,'') <> 'passed'");

            var result = PurchaseService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewBillNo()
        {
            var service = new psi_purchaseService();
            return service.GetNewKey("BillNo", "dateplus");
        }

        public void Delete(string id)
        {
            var service = new psi_purchaseService();
            var result = service.Delete(ParamDelete.Instance().AndWhere("BillNo", id));
            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("采购单删除失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, dynamic data)
        {
            var service = new psi_purchaseService();
            var result = service.Update(ParamUpdate.Instance()
                .Column("AuditState", data.status)
                .Column("AuditReason", data.comment)
                .Column("AuditPerson", FormsAuth.GetUserData().UserName)
                .Column("AuditDate", DateTime.Now)
                .AndWhere("BillNo", id));

            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("审核采购单失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }
        #endregion

        #region 编辑页面用 api
        public dynamic GetEditMaster(string id)
        {
            dynamic result = new ExpandoObject();
            var masterService = new psi_purchaseService();
            result.form = masterService.GetDynamic(ParamQuery.Instance().Select("*,cast((case when AuditState='passed' then 'true' else 'false' end) as bit) as IsEffect ").AndWhere("BillNo", id));
            if (result.form!=null) result.form.BillNo = id; //如果没有数据，返回一条空数据，BillNo=id; 对应新增的情况
            result.scrollKeys = masterService.ScrollKeys("BillNo", id);
            return result;
        }

        public string GetNewRowId(int id)
        {
            var service = new psi_purchaseGoodsService();
            return service.GetNewKey("RowId", "maxplus", id);
        }

        public dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>
        A.*, B.Name,B.Catagory,B.Brand,B.Model,B.Unit
    </select>
    <from>
        psi_purchaseGoods A
        left join psi_goods B on B.Id = A.GoodNo
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var PurchaseService = new psi_purchaseService();
            var result = PurchaseService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_purchase
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_purchaseGoods
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>");

            var service = new psi_purchaseService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}