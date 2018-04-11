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
    public class RefundController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("refund"),
                resx = MmsHelper.GetIndexResx("退库单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    RefundUnitName = "",
                    RefundPerson = "",
                    WarehouseCode = "",
                    MaterialType = "",
                    RefundDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new RefundApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("refund"),
                resx = MmsHelper.GetEditResx("退库单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_refund().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    RefundDate = DateTime.Now
                }),
                defaultRow = new
                {
                    Num = 1,
                    UnitPrice = 0,
                    TotalMoney = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "UnitPrice", "Num", "Money", "Remark","SrcBillType","SrcBillNo","SrcRowId" }
                }
            };

            return View(model);
        }
    }

    public class RefundApiController : MmsBaseApi<mms_refund, mms_refundService, mms_refundDetail, mms_refundDetailService>
    {
        // 地址：GET api/mms/refund/getdoperson
        public List<dynamic> GetRefundPerson(string q)
        {
            var SendService = new mms_refundService();
            var pQuery = ParamQuery.Instance().Select("top 10 RefundPerson").AndWhere("RefundPerson", q, Cp.StartWithPY);
            return SendService.GetDynamicList(pQuery);
        }

        // 地址：GET api/mms/refund
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName, D.WarehouseName,E.MerchantsName as RefundUnitName
    </select>
    <from>
        mms_refund A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType = A.MaterialType
        left join mms_warehouse         D on D.WarehouseCode       = A.WarehouseCode
        left join mms_merchants     E on E.MerchantsCode    = A.RefundUnit
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'              cp='equal'      ></field>
        <field name='ProjectName'         cp='like'       ></field>
        <field name='E.MerchantsName'     cp='like' variable='RefundUnitName'      ></field>
        <field name='A.WarehouseCode'             cp='equal'   variable='WarehouseCode'   ></field>
        <field name='A.MaterialType'  cp='equal'      ></field>
        <field name='RefundDate'          cp='daterange'  ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }

        public override dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
    <select>
        A.*, B.MaterialName,B.Model,B.Material,C.UnitPrice as SrcUnitPrice,C.Num as SrcNum
    </select>
    <from>
        mms_refundDetail A
        left join mms_material B on B.MaterialCode = A.MaterialCode
        left join mms_sendDetail C on C.BillNo =  A.SrcBillNo and C.RowId = A.SrcRowId
    </from>
    <where>
        <field name='A.BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}