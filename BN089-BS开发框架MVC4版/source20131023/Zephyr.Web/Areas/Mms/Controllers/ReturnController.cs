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

    public class ReturnController : Controller
    {
        public ActionResult Index()
        {
            var codeService = new sys_codeService();
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("return"),
                resx = MmsHelper.GetIndexResx("退货单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject()),
                    priceKinds = codeService.GetValueTextListByType("Pricing"),
                    payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    BillNo = "",
                    ReturnUnitName = "",
                    MerchantsName = "",
                    WarehouseCode = "",
                    PriceKind = "",
                    ReturnDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new ReturnApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("return"),
                resx = MmsHelper.GetEditResx("退货单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    priceKinds = codeService.GetValueTextListByType("Pricing"),
                    payKinds = codeService.GetValueTextListByType("PayType"),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_return().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    ReturnDate = DateTime.Now,
                    PayKind = codeService.GetDefaultCode("PayType"),
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
                    postListFields = new string[] {  "BillNo", "RowId", "MaterialCode", "Unit",  "UnitPrice", "Num","Money", "Remark", "SrcBillType","SrcBillNo","SrcRowId" }
                }
            };
            return View(model);
        }
    }

    public class ReturnApiController : MmsBaseApi<mms_return, mms_returnService, mms_returnDetail, mms_returnDetailService>
    {
        // 查询主表：GET api/mms/send
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName, D.WarehouseName,E.MerchantsName
    </select>
    <from>
        mms_return A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType     = A.MaterialType
        left join mms_warehouse     D on D.WarehouseCode    = A.WarehouseCode
        left join mms_merchants     E on E.MerchantsCode    = A.SupplierCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'              cp='equal'      ></field>
        <field name='E.MerchantsName'     cp='like'       ></field>
        <field name='A.WarehouseCode'       cp='equal' variable='WarehouseCode'     ></field>
        <field name='A.PriceKind'      cp='equal' variable='PriceKind'      ></field>
        <field name='ReturnDate'          cp='daterange'  ></field>
        <field name='DoPerson'       cp='like'      ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }
    }
}