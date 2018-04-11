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
    public class RentinController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("rentin"),
                resx = MmsHelper.GetIndexResx("租赁进场单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ProjectName = "",
                    DoPerson = "",
                    MerchantsName = "",
                    ContractCode = "",
                    RentInDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new RentinApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("rentin"),
                resx = MmsHelper.GetEditResx("租赁进场单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType(),
                    materialUse = codeService.GetValueTextListByType("MaterialUse"),
                    billingUnit = codeService.GetValueTextListByType("BillingUnit")
                },
                defaultForm = new mms_rentIn().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    RentInDate = DateTime.Now,
                    Purpose = codeService.GetDefaultCode("MaterialUse")
                }),
                defaultRow = new
                {
                    Num = 1,
                    UnitPrice = 1
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "MaterialCode", "Unit", "UseUnit", "PriceUnit", "Num", "UnitPrice", "Remark" }
                }
            };
            
            return View(model);
        }
    }

    public class RentinApiController : MmsBaseApi<mms_rentIn, mms_rentInService, mms_rentInDetail, mms_rentInDetailService>
    {
        // 地址：GET api/mms/rentin/getdoperson
        public List<dynamic> GetDoPerson(string q)
        {
            var SendService = new mms_rentInService();
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
        mms_rentIn A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_merchants     C on C.MerchantsCode    = A.SupplierCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'             cp='equal'      ></field>
        <field name='ProjectName'        cp='like'       ></field>
        <field name='DoPerson'           cp='like'       ></field>
        <field name='C.MerchantsName'    cp='like'       ></field>
        <field name='ContractCode'       cp='equal'      ></field>
        <field name='RentInDate'         cp='daterange'  ></field>
    </where>
</settings>");
            return base.Get(query);
        }

        public override dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
    <select>
        A.*, B.MaterialName,B.Model,B.Material,C.MerchantsName as UseUnitName
    </select>
    <from>
        mms_rentInDetail A
        left join mms_material B on B.MaterialCode      = A.MaterialCode
        left join mms_merchants C on C.MerchantsCode    = A.UseUnit
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}