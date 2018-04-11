using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Zephyr.Web;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class TransferController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("transfer"),
                resx = MmsHelper.GetIndexResx("调拨单"),
                dataSource = new
                {
                    PriceKind = new sys_codeService().GetDynamicList(ParamQuery.Instance().Select("Code as value,Text as text").AndWhere("CodeType", "Pricing")),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ReceiveUnitName = "",
                    DoPerson = "",
                    WarehouseCode = "",
                    PriceKind = "",
                    TransferDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new TransferApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("transfer"),
                resx = MmsHelper.GetEditResx("调拨单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    supplyType = codeService.GetValueTextListByType("SupplyType"),
                    payKinds = codeService.GetValueTextListByType("PayType"),
                    priceKinds = codeService.GetValueTextListByType("Pricing"),
                    //materialUse = codeService.GetValueTextListByType("MaterialUse"),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_transfer().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    TransferDate = DateTime.Now,
                    PriceKind = codeService.GetDefaultCode("Pricing")
                }),
                defaultRow = new
                {
                    Num = 1,
                    UnitPrice = 0,
                    Money = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "UnitPrice", "Num", "Money", "Remark", "SrcBillType", "SrcBillNo", "SrcRowId" }
                }
            };

            return View(model);
        }
    }

    public class TransferApiController : MmsBaseApi<mms_transfer, mms_transferService, mms_transferDetail, mms_transferDetailService>
    {
        // 地址：GET api/mms/transfer/getdoperson
        public List<dynamic> GetDoPerson(string q)
        {
            var SendService = new mms_transferService();
            var pQuery = ParamQuery.Instance().Select("top 10 DoPerson").AndWhere("DoPerson", q, Cp.StartWithPY);
            return SendService.GetDynamicList(pQuery);
        }

        // 地址：GET api/mms/transfer
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName, D.WarehouseName,E.MerchantsName as ReceiveUnitName
    </select>
    <from>
        mms_transfer A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType = A.MaterialType
        left join mms_warehouse         D on D.WarehouseCode       = A.WarehouseCode
        left join mms_merchants     E on E.MerchantsCode    = A.ReceiveUnit
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'              cp='equal'      ></field>
        <field name='E.MerchantsName'         cp='like' variable='ReceiveUnitName'      ></field>
        <field name='A.WarehouseCode'             cp='equal' variable='WarehouseCode'     ></field>
        <field name='A.PriceKind'  cp='equal'  variable='PriceKind'     ></field>
        <field name='TransferDate'        cp='daterange'  ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }


        // 地址：GET api/mms/deal/getdetail
        public override dynamic GetDetail(string id)
        {
            var sfrom = @" 
    select 'receive' as SrcBillType
    ,A.BillNo as SrcBillNo
    ,A.RowId as SrcRowId
    ,B.ReceiveDate as SrcDate
    ,B.WarehouseCode
    ,B.ProjectCode
    ,A.RemainNum
    ,A.UnitPrice as SrcUnitPrice
    ,C.*
    from mms_receiveDetail as A
    left join mms_receive as B ON A.BillNo=B.BillNo
    left join mms_material as C ON A.MaterialCode=C.MaterialCode

    union

    select 'refund' as SrcBillType
    ,A.BillNo as SrcBillNo
    ,A.RowId as SrcRowId
    ,B.RefundDate as SrcDate
    ,B.WarehouseCode
    ,B.ProjectCode
    ,A.RemainNum
    ,A.UnitPrice as SrcUnitPrice
    ,C.*
    from mms_refundDetail as A
    left join mms_refund as B ON A.BillNo=B.BillNo
    left join mms_material as C ON A.MaterialCode=C.MaterialCode

    union

    select 'adjust' as SrcBillType
    ,A.BillNo as SrcBillNo
    ,A.RowId as SrcRowId
    ,B.EffectDate as SrcDate
    ,B.WarehouseCode
    ,B.ProjectCode
    ,A.RemainNum
    ,A.UnitPrice as SrcUnitPrice
    ,C.*
    from mms_stockAdjustDetail as A
    left join mms_stockAdjust as B ON A.BillNo=B.BillNo
    left join mms_material as C ON A.MaterialCode=C.MaterialCode
 ";

            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>
        A.*, C.MaterialName,C.Model,C.Material,D.Num as StockNum,D.UnitPrice as StockUnitPrice,T.RemainNum,T.SrcUnitPrice
    </select>
    <from>
        mms_transferDetail A
        left join mms_transfer B on B.BillNo = A.BillNo
        left join mms_material C on C.MaterialCode = A.MaterialCode
        left join mms_warehouseStock D on D.WarehouseCode = B.WarehouseCode and D.MaterialCode = A.MaterialCode
        left join ({0}) T on T.SrcBillType=A.SrcBillType and T.SrcBillNo=A.SrcBillNo and T.SrcRowId = A.SrcRowId
    </from>
    <where>
        <field name='A.BillNo' cp='equal'></field>
    </where>
</settings>", sfrom);

            var pQuery = query.ToParamQuery();
            var result = detailService.GetDynamicListWithPaging(pQuery);
            return result;
        }



    }
}