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
    public class DirectController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("direct"),
                resx = MmsHelper.GetIndexResx("直入直出单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject()),
                    supplyType = new sys_codeService().GetValueTextListByType("SupplyType")
                },
                form = new
                {
                    BillNo = "",
                    ContractCode = "",
                    SupplierName = "",
                    PickUnitName = "",
                    SupplyType = "",
                    ArriveDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new DirectApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("direct"),
                resx = MmsHelper.GetEditResx("直入直出单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    supplyType = codeService.GetValueTextListByType("SupplyType"),
                    payKinds = codeService.GetValueTextListByType("PayType"),
                    materialUse = codeService.GetValueTextListByType("MaterialUse")
                },
                defaultForm = new mms_direct().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    ArriveDate = DateTime.Now,
                    SupplyType = codeService.GetDefaultCode("SupplyType"),
                    PayKind = codeService.GetDefaultCode("PayType")
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
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "UnitPrice", "Num", "Money", "Remark"}
                }
            };

            return View(model);
        }
    }

    public class DirectApiController : MmsBaseApi<mms_direct, mms_directService, mms_directDetail, mms_directDetailService>
    {
        // 地址：GET api/mms/direct
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName,D.MerchantsName as SupplierName,E.MerchantsName as PickUnitName
    </select>
    <from>
        mms_direct A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType     = A.MaterialType
        left join mms_merchants     D on D.MerchantsCode    = A.SupplierCode
        left join mms_merchants     E on E.MerchantsCode    = A.PickUnit
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'                cp='equal'      ></field>
        <field name='ContractCode'           cp='like'       ></field>
        <field name='D.MerchantsName'       cp='like'  variable='SupplierName'     ></field>
        <field name='E.MerchantsName'       cp='like'  variable='PickUnitName'     ></field>
        <field name='A.MaterialType'        cp='equal'      ></field>
        <field name='ArriveDate'            cp='daterange'  ></field>
        <field name='SupplyType'            cp='equal'  ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }
    }
}