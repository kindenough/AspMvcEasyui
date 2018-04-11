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
    public class RentoutController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("rentout"),
                resx = MmsHelper.GetIndexResx("租赁出场单"),
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(MmsHelper.GetCurrentProject())
                },
                form = new
                {
                    BillNo = "",
                    ProjectName = "",
                    MerchantsName = "",
                    ContractCode = "",
                    IsTotal = false,
                    RentOutDate = ""
                }
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var userName = MmsHelper.GetUserName();
            var currentProject = MmsHelper.GetCurrentProject();
            var data = new RentoutApiController().GetEditMaster(id);
            var codeService = new sys_codeService();

            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                urls = MmsHelper.GetEditUrls("rentout"),
                resx = MmsHelper.GetEditResx("租赁出场单"),
                dataSource = new
                {
                    measureUnit = codeService.GetMeasureUnitListByType()
                },
                defaultForm = new mms_rentOut().Extend(new
                {
                    BillNo = id,
                    BillDate = DateTime.Now,
                    DoPerson = userName,
                    RentOutDate = DateTime.Now
                }),
                defaultRow = new
                {
                    CheckNum = 1,
                    InStoreNum = 1,
                    InStoreUnitPrice = 0,
                    TotalMoney = 0,
                    PrePay = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "SrcBillNo", "SrcRowId", "MaterialCode", "Unit", "Num", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class RentoutApiController : MmsBaseApi<mms_rentOut, mms_rentOutService, mms_rentOutDetail, mms_rentOutDetailService>
    {
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*, B.ProjectName, C.MerchantsName
    </select>
    <from>
        mms_rentOut A
        left join mms_project       B on B.ProjectCode      = A.ProjectCode
        left join mms_merchants     C on C.MerchantsCode    = A.SupplierCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'             cp='equal'      ></field>
        <field name='ProjectName'        cp='like'       ></field>
        <field name='C.MerchantsName'    cp='like'       ></field>
        <field name='ContractCode'       cp='like'       ></field>
        <field name='RentOutDate'        cp='daterange'  ></field>
    </where>
</settings>");
            var pQuery = query.ToParamQuery().AndWhere("IsTotal", query["IsTotal"], 
                x => "true".Equals(x.Value) ? "IsTotal=1" : "isnull(IsTotal,0)<>1");
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        // 地址：GET api/mms/rentout/getdetail
        public override dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
    <select>
        A.*, B.MaterialName,B.Model,B.Material 
        ,C.RemainNum,C.RowId as SrcRowId,C.BillNo as SrcBillNo
    </select>
    <from>
        mms_rentOutDetail A
        left join mms_material B on B.MaterialCode = A.MaterialCode
        left join mms_rentInDetail C ON C.BillNo=A.SrcBillNo AND C.RowId=A.SrcRowId
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