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
    public class RentOffController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("rentoff"),
                resx = MmsHelper.GetIndexResx("停租单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ProjectName = "",
                    DoPerson = "",
                    ContractCode = "",
                    BeginDate = "",
                    EndDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new RentOffApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("rentOff"),
                resx = MmsHelper.GetEditResx("停租单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
                },
                defaultForm = new mms_rentOff().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    BeginDate = DateTime.Now
                }),
                defaultRow = new
                {
                    UnitPrice = 0,
                    Num = 1,
                    Day = 1,
                    PrePay = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "UnitPrice", "Num", "Day", "Money", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class RentOffApiController : MmsBaseApi<mms_rentOff, mms_rentOffService, mms_rentOffDetail, mms_rentOffDetailService>
    {
        // 地址：GET api/mms/send/getdoperson
        public List<dynamic> GetDoPerson(string q)
        {
            var SendService = new mms_rentOffService();
            var pQuery = ParamQuery.Instance().Select("top 10 DoPerson").AndWhere("DoPerson", q, Cp.StartWithPY);
            return SendService.GetDynamicList(pQuery);
        }
 
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MerchantsName
    </select>
    <from>
        mms_rentOff A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_merchants     C on C.MerchantsCode    = A.SupplierCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'             cp='equal'      ></field>
        <field name='ProjectName'        cp='like'       ></field>
        <field name='DoPerson'           cp='like'       ></field>
        <field name='ContractCode'       cp='equal'      ></field>
        <field name='BeginDate'          cp='daterange'  ></field>
        <field name='EndDate'            cp='daterange'  ></field>
    </where>
</settings>");

            return base.Get(query);
        }
    }
}