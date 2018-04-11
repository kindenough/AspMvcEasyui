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
    public class ReceiveController : Controller
    {
        #region 查询页面
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    query = "/api/psi/receive",
                    remove = "/api/psi/receive/",
                    billno = "/api/psi/receive/getnewbillno",
                    audit = "/api/psi/receive/audit/",
                    edit = "/psi/receive/edit/"
                },
                resx = new
                {
                    detailTitle = "收货单明细",
                    noneSelect = "请先选择一条收货单！",
                    deleteConfirm = "确定要删除选中的收货单吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    BillNo = "",
                    ReceivePerson = "",
                    SupplierName = "",
                    Warehouse = "",
                    Contract = "",
                    ReceiveDate = ""
                }
            };

            return View(model);
        }
        #endregion

        #region 编辑页面
        public ActionResult Edit(string id = "")
        {
            var data = new ReceiveApiController().GetEditMaster(id);
            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                dataSource = new
                {
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    getdetail = "/api/psi/receive/getdetail/",
                    getmaster = "/api/psi/receive/geteditmaster/",
                    edit = "/api/psi/receive/edit",
                    audit = "/api/psi/receive/audit/",
                    getrowid = "/api/psi/receive/getnewrowid/",
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
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

    public class ReceiveApiController : ApiController
    {
        #region 查询页面 api
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*,B.Name as WarehouseName,C.Name as SupplierName
    </select>
    <from>
        psi_receive A
        left join psi_warehouse B on B.Id = A.Warehouse
        left join psi_supplier  C on C.Id = A.Supplier
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'            cp='equal'      ></field>
        <field name='Warehouse'         cp='equal'       ></field>
        <field name='ReceivePerson'     cp='like'       ></field>
        <field name='Contract'          cp='like'       ></field>
        <field name='ReceiveDate'       cp='daterange'       ></field>
        <field name='C.Name'            cp='like' variable='SupplierName'></field>
    </where>
</settings>");

            var ReceiveService = new psi_receiveService();
            var pQuery = query.ToParamQuery();
            var result = ReceiveService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewBillNo()
        {
            var service = new psi_receiveService();
            return service.GetNewKey("BillNo", "dateplus");
        }

        public void Delete(string id)
        {
            var service = new psi_receiveService();
            var result = service.Delete(ParamDelete.Instance().AndWhere("BillNo", id));
            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("收货单删除失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, dynamic data)
        {
            var service = new psi_receiveService();
            var result = service.Update(ParamUpdate.Instance()
                .Column("AuditState", data.status)
                .Column("AuditReason", data.comment)
                .Column("AuditPerson", FormsAuth.GetUserData().UserName)
                .Column("AuditDate", DateTime.Now)
                .AndWhere("BillNo", id));

            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("审核收货单失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }
        #endregion

        #region 编辑页面用 api
        public dynamic GetEditMaster(string id)
        {
            dynamic result = new ExpandoObject();
            var masterService = new psi_receiveService();
            result.form = masterService.GetModel(ParamQuery.Instance().AndWhere("BillNo", id));
            result.form.BillNo = id; //如果没有数据，返回一条空数据，BillNo=id; 对应新增的情况
            result.scrollKeys = masterService.ScrollKeys("BillNo", id);
            return result;
        }

        public string GetNewRowId(int id)
        {
            var service = new psi_receiveGoodsService();
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
        psi_receiveGoods A
        left join psi_goods B on B.Id = A.GoodNo
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var ReceiveService = new psi_receiveService();
            var result = ReceiveService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_receive
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_receiveGoods
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>");

            var service = new psi_receiveService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}