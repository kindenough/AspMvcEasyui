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
    public class CheckController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("check"),
                resx = MmsHelper.GetIndexResx("盘点单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    IsShowDiff = false,
                    DoPerson = "",
                    WarehouseCode = "",
                    MaterialType = "",
                    BillDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new CheckApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("check"),
                resx = MmsHelper.GetEditResx("盘点单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject),
                    yearItems = codeService.GetYearItems(),
                    monthItems = codeService.GetMonthItems()
                },
                defaultForm = new mms_check().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    Years = DateTime.Now.Year,
                    Months = DateTime.Now.Month
                }),
                defaultRow = new
                {
                    BookNum = 0,
                    BookMoney = 0,
                    ActualNum = 0,
                    ActualUnitPrice = 0,
                    OperateNum = 0,
                    OperateMoney = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "BookNum", "BookMoney", "ActualNum", "ActualUnitPrice", "OperateNum", "OperateMoney", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class CheckApiController : MmsBaseApi<mms_check, mms_checkService, mms_checkDetail, mms_checkDetailService>
    {
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
        mms_check A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_materialType  C on C.MaterialType = A.MaterialType
        left join mms_warehouse         D on D.WarehouseCode       = A.WarehouseCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'             cp='equal'      ></field>
        <field name='ProjectName'        cp='like'       ></field>
        <field name='DoPerson'           cp='like'       ></field>
        <field name='A.WarehouseCode'     cp='equal' variable='WarehouseCode'></field>
        <field name='A.MaterialType' cp='equal'      ></field>
        <field name='CheckDate'          cp='daterange'  ></field>
    </where>
</settings>");
            //if(query["IsShowDiff"] == "true")
            //    var a =0;

            var pQuery = query.ToParamQuery().AndWhere("IsShowDiff", query["IsShowDiff"],
                x => "true".Equals(x.Value) ? "isnull(A.BookMoney,0)<>isnull(A.OperateMoney,0)" : "1=1").AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
                //.AndWhere("CheckDate", query["CheckDate"], x => x == null ? "1=1" : string.Format("CheckDate", x.Value));
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}