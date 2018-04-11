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
    public class PickingController : Controller
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
                    query = "/api/psi/picking",
                    remove = "/api/psi/picking/",
                    billno = "/api/psi/picking/getnewbillno",
                    audit = "/api/psi/picking/audit/",
                    edit = "/psi/picking/edit/"
                },
                resx = new{
                    detailTitle = "领货单明细",
                    noneSelect = "请先选择一条领货单！",
                    deleteConfirm = "确定要删除选中的领货单吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    BillNo = "",
                    PickPerson = "",
                    PickDate = "",
                    CustomerName = "",
                    Warehouse = "",
                    ContractNo=""
                }
            };

            return View(model);
        }
        #endregion

        #region 编辑页面
        public ActionResult Edit(string id = "")
        {
            var data = new PickingApiController().GetEditMaster(id);
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
                    getdetail = "/api/psi/picking/getdetail/",
                    getmaster = "/api/psi/picking/geteditmaster/",
                    edit = "/api/psi/picking/edit",
                    audit = "/api/psi/picking/audit/",
                    getrowid = "/api/psi/picking/getnewrowid/",
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

    public class PickingApiController : ApiController
    {
        #region 查询页面 api
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*,B.Name as WarehouseName,C.Name as CustomerName
    </select>
    <from>
        psi_picking A
        left join psi_warehouse B on B.Id = A.Warehouse
        left join psi_customer  C on C.Id = A.CustomNo
        left join psi_customerContract D on D.ContractNo = A.Contract
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'            cp='equal'      ></field>
        <field name='Warehouse'         cp='equal'       ></field>
        <field name='PickingPerson'     cp='like'       ></field>
        <field name='PickingDate'       cp='daterange'       ></field>
        <field name='C.Name'            cp='like' variable='CustomerName'></field>
        <field name='Contract'          cp='like'       ></field>
    </where>
</settings>");  

            var PickingService = new psi_pickingService();
            var pQuery = query.ToParamQuery();
            var result = PickingService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewBillNo()
        {
            var service = new psi_pickingService();
            return service.GetNewKey("BillNo", "dateplus");
        }

        public void Delete(string id)
        {
            var service = new psi_pickingService();
            var result = service.Delete(ParamDelete.Instance().AndWhere("BillNo", id));
            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("领货单删除失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, dynamic data)
        {
            var service = new psi_pickingService();
            var result = service.Update(ParamUpdate.Instance()
                .Column("AuditState", data.status)
                .Column("AuditReason", data.comment)
                .Column("AuditPerson", FormsAuth.GetUserData().UserName)
                .Column("AuditDate", DateTime.Now)
                .AndWhere("BillNo", id));

            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("审核领货单失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }
        #endregion 

        #region 编辑页面用 api
        public dynamic GetEditMaster(string id) {
            dynamic result = new ExpandoObject();
            var masterService = new psi_pickingService();
            result.form = masterService.GetModel(ParamQuery.Instance().AndWhere("BillNo", id));
            result.form.BillNo = id; //如果没有数据，返回一条空数据，BillNo=id; 对应新增的情况
            result.scrollKeys = masterService.ScrollKeys("BillNo", id);
            return result;
        }
 
        public string GetNewRowId(int id)
        {
            var service = new psi_pickingGoodsService();
            return service.GetNewKey("RowId", "maxplus",id);
        }
  
        public dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>
        A.*, B.Name,B.Catagory,B.Brand,B.Model,B.Unit
    </select>
    <from>
        psi_pickingGoods A
        left join psi_goods B on B.Id = A.GoodNo
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var PickingService = new psi_pickingService();
            var result = PickingService.GetDynamicListWithPaging(pQuery);
            return result;
        }
 
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_picking
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_pickingGoods
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>");
             
            var service = new psi_pickingService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}