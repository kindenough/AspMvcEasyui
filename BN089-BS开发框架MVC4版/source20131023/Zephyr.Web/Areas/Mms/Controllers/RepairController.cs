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
    public class RepairController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("repair"),
                resx = MmsHelper.GetIndexResx("维修单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ProjectName = "",
                    DoPerson = "",
                    RepairUnit = "",
                    RepairDate = "",
                    Remark = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new RepairApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("repair"),
                resx = MmsHelper.GetEditResx("维修单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    supplyType = codeService.GetValueTextListByType("SupplyType"),
                    payKinds = codeService.GetValueTextListByType("PayType"),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_repair().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    RepairDate = DateTime.Now
                }),
                defaultRow = new
                {
                    LaborCost = 0,
                    UnitPrice = 0,
                    Num = 0,
                    TotalMoney = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "RepairNo", "RepairName", "Unit", "LaborCost", "UnitPrice", "Num", "Money", "RepairReason", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class RepairApiController : MmsBaseApi<mms_repair, mms_repairService, mms_repairDetail, mms_repairDetailService>
    {
        // 地址：GET api/mms/repair/getdoperson
        public List<dynamic> GetDoPerson(string q)
        {
            var SendService = new mms_repairService();
            var pQuery = ParamQuery.Instance().Select("top 10 DoPerson").AndWhere("DoPerson", q, Cp.StartWithPY);
            return SendService.GetDynamicList(pQuery);
        }
 
        // 地址：GET api/mms/repair
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MerchantsName
    </select>
    <from>
        mms_repair A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_merchants     C on C.MerchantsCode    = A.RepairUnit
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'              cp='equal'      ></field>
        <field name='ProjectName'         cp='like'       ></field>
        <field name='DoPerson'            cp='like'       ></field>
        <field name='RepairUnit'        cp='equal'       ></field>
        <field name='RepairDate'          cp='daterange'  ></field>
        <field name='A.Remark'            cp='like'      ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery().AndWhere("A.ProjectCode", MmsHelper.GetCurrentProject());
            return masterService.GetDynamicListWithPaging(pQuery);
        }

        // 地址：GET api/mms/repair/getdetail
        public override dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RepairNo'>
    <select>
        *
    </select>
    <from>
        mms_repairDetail 
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var result = detailService.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}