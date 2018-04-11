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
    public class LossreportController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("lossreport"),
                resx = MmsHelper.GetIndexResx("报损单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ProjectName = "",
                    DoPerson = "",
                    WarehouseCode = "",
                    MaterialType = "",
                    LossReportDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new LossreportApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("lossreport"),
                resx = MmsHelper.GetEditResx("报损单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_lossReport().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    LossReportDate = DateTime.Now 
                }),
                defaultRow = new
                {
                    UnitPrice = 0,
                    Num = 1,
                    Money = 0
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

    public class LossreportApiController : MmsBaseApi<mms_lossReport, mms_lossReportService, mms_lossReportDetail, mms_lossReportDetailService>
    {
        // 地址：GET api/mms/lossreport/getdoperson
        public List<dynamic> GetDoPerson(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 DoPerson").AndWhere("DoPerson", q, Cp.StartWithPY);
            return masterService.GetDynamicList(pQuery);
        }
 
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MaterialTypeName, D.WarehouseName
    </select>
    <from>
        mms_LossReport A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType = A.MaterialType
        left join mms_warehouse         D on D.WarehouseCode       = A.WarehouseCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'              cp='equal'      ></field>
        <field name='ProjectName'         cp='like'       ></field>
        <field name='DoPerson'            cp='like'       ></field>
        <field name='WarehouseCode'             cp='equal'      ></field>
        <field name='A.MaterialType'  cp='equal'      ></field>
        <field name='LossReportDate'      cp='daterange'  ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }

        // 地址：GET api/mms/deal/getdetail
        public override dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>
        A.*, C.MaterialName,C.Model,C.Material,D.Num as StockNum,D.UnitPrice as StockUnitPrice
    </select>
    <from>
        mms_LossReportDetail A
        left join mms_LossReport B on B.BillNo = A.BillNo
        left join mms_material C on C.MaterialCode = A.MaterialCode
        left join mms_warehouseStock D on D.WarehouseCode = B.WarehouseCode and D.MaterialCode = A.MaterialCode
    </from>
    <where>
        <field name='A.BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var result = detailService.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}