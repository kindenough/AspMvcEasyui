using System;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class ReceiveController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("receive"),
                resx = MmsHelper.GetIndexResx("收料单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject()),
                    supplyType = new sys_codeService().GetValueTextListByType("SupplyType")
                },
                form = new
                {
                    BillNo = "",
                    //ProjectName = "",
                    SupplierName = "",
                    SupplyType = "",
                    WarehouseCode = "",
                    ContractCode="",
                    //MaterialType = "",
                    ReceiveDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new ReceiveApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("receive"),
                resx = MmsHelper.GetEditResx("收料单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    supplyType = codeService.GetValueTextListByType("SupplyType"),
                    payKinds = codeService.GetValueTextListByType("PayType"),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_receive().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    ReceiveDate = DateTime.Now,
                    SupplyType = codeService.GetDefaultCode("SupplyType"),
                    PayKind = codeService.GetDefaultCode("PayType"),
                }),
                defaultRow = new
                {
                    CheckNum = 1,
                    Num = 1,
                    UnitPrice = 0,
                    Money = 0,
                    PrePay = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "CheckNum", "Num", "UnitPrice", "PrePay", "Money", "Remark" }
                }
            };
            return View(model);
        }
    }

    public class ReceiveApiController : MmsBaseApi<mms_receive, mms_receiveService, mms_receiveDetail, mms_receiveDetailService>
    {
        // 查询主表：GET api/mms/send
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName, D.WarehouseName as WarehouseName, E.MerchantsName AS SupplierName
    </select>
    <from>
        mms_receive A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType = A.MaterialType
        left join mms_warehouse         D on D.WarehouseCode       = A.WarehouseCode
        left join mms_merchants     E on E.MerchantsCode    = A.SupplierCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'                cp='equal'      ></field>
        <field name='DoPerson'           cp='like'       ></field>
        <field name='E.MerchantsName'       cp='like'    variable='SupplierName'   ></field>
        <field name='A.WarehouseCode'       cp='equal'      ></field>
        <field name='A.MaterialType'        cp='equal'      ></field>
        <field name='ReceiveDate'           cp='daterange'  ></field>
        <field name='ContractCode'           cp='like'  ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());

            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
            //return base.Get(query);
        }
    }
}