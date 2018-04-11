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
using System.Web;

namespace Zephyr.Areas.Mms.Controllers
{
    public class ReportController : Controller
    {
        //收料情况分析（供应商角度）
        public ActionResult ReceiveSummary()
        {

            var model = new
            {
                dataSource = new
                {
                    billTypeItems = MmsService.GetReceiveSrcBillTypeList()
                },
                form = new {
                    SupplierName = "",
                    ContractCode = "",
                    QueryDate = "",
                    SrcBillType = "",
                    MaterialType = "",
                    MaterialName = ""
                },
                urls = new {
                    query = "/api/mms/report/GetReceiveSummary/"
                }
                
            };

            return View(model);
        }

        //发料情况分析（领料单位角度）
        public ActionResult SendSummary()
        {
            var currentProjectCode = MmsHelper.GetCurrentProject();

            var model = new
            {
                dataSource = new
                {
                    billTypeItems = MmsService.GetSendSrcBillTypeList(),
                    buildPartItems = new mms_buildPartService().GetBuildPartItems(currentProjectCode)
                },
                form = new
                {
                    PickUnitName = "",
                    BuildPartCode = "",
                    QueryDate = "",
                    SrcBillType = "",
                    MaterialType = "",
                    MaterialName = ""
                },
                urls = new
                {
                    query = "/api/mms/report/GetSendSummary/"
                }

            };

            return View(model);
        }

        //库存查询（可查历史）
        public ActionResult WarehouseQuery()
        {
            var currentProjectCode = MmsHelper.GetCurrentProject();

            var model = new
            {
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProjectCode)
                },
                form = new
                {
                    WarehouseCode = "",
                    MaterialName = ""
                },
                urls = new
                {
                    query = "/api/mms/report/GetWarehouseQuery/"
                }
            };

            return View(model);
        }

        //库存流水查询
        public ActionResult WarehouseAccount()
        {
            var currentProjectCode = MmsHelper.GetCurrentProject();

            var model = new
            {
                dataSource = new
                {
                    billTypeItems = MmsService.GetAccountSrcBillTypeList()
                },
                form = new
                {
                    SrcBillType = "",
                    CreatePerson = "",
                    SrcBillNo = "",
                    QueryDate = "",
                    MaterialType = "",
                    MaterialName = ""
                },
                urls = new
                {
                    query = "/api/mms/report/GetWarehouseAccount/"
                }

            };

            return View(model);
        }

        //材料预警提示
        public ActionResult MaterialWarnning()
        {
            //var currentProjectCode = MmsHelper.GetCurrentProject();

            var model = new
            {
                dataSource = new
                {

                },
                form = new
                {
                    MaterialType = "",
                    MaterialName = "",
                    IsWarnning = ""
                },
                urls = new
                {
                    query = "/api/mms/report/GetMaterialWarnning/",
                    queryDetail = "/api/mms/report/GetMaterialDetail/"
                }

            };

            return View(model);
        }

        //收发-结存报表
        public ActionResult BalanceAccount()
        {
            var currentProjectCode = MmsHelper.GetCurrentProject();

            var model = new
            {
                dataSource = new
                {
                    warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProjectCode),
                    buildPartItems = new mms_buildPartService().GetBuildPartItems(currentProjectCode)
                },
                form = new
                {
                    WarehouseCode = "",
                    MaterialName = "",
                    PickUnitName = "",
                    BuildPartCode = "",
                    QueryDate= ""
                },
                urls = new
                {
                    query = "/api/mms/report/GetBalanceAccount/"
                }

            };

            return View(model);
        }
    }

    public class ReportApiController : ApiController
    {
        //收料情况分析（供应商角度）
        public dynamic GetReceiveSummary(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();

            string SupplierName = query["SupplierName"] ?? string.Empty;
            string ContractCode = query["ContractCode"] ?? string.Empty;
            string SrcBillType = query["SrcBillType"] ?? string.Empty;
            string MaterialType = query["MaterialType"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string QueryDate = query["QueryDate"] ?? string.Empty;
            string sWhere ="";
            var result = new List<dynamic>();

            Dictionary<string,string> sCondition = new Dictionary<string,string>();
            sCondition.Add("receive", string.Format(" And  mms_receive.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("return", string.Format(" And  mms_return.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("rentin", string.Format(" And  mms_rentin.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("direct", string.Format(" And  mms_direct.ProjectCode='{0}'", CurrentProjectCode));

            if (SupplierName.Length > 0)
            {
                sCondition["receive"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", SupplierName);
                sCondition["rentin"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", SupplierName);
                sCondition["return"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", SupplierName);
                sCondition["direct"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", SupplierName);
            }

            if (ContractCode.Length > 0)
            {
                sCondition["receive"] += string.Format(" And mms_receive.ContractCode like '%{0}%'", ContractCode);
                sCondition["rentin"] += string.Format(" And mms_rentin.ContractCode like '%{0}%'", ContractCode);
                sCondition["return"] += string.Format(" And mms_return.ContractCode like '%{0}%'", ContractCode);
                sCondition["direct"] += string.Format(" And mms_direct.ContractCode like '%{0}%'", ContractCode);
            }

            if (MaterialName.Length > 0)
            {
                sCondition["receive"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["rentin"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["return"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["direct"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            if (SrcBillType.Length > 0)
            {
                sWhere += string.Format(" And T.SrcBillType='{0}'", SrcBillType);
            }

            if (QueryDate.Length > 0)
            {
                sCondition["receive"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_receive.ReceiveDate", Value = QueryDate });
                sCondition["rentin"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_rentIn.RentInDate", Value = QueryDate });
                sCondition["return"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_return.ReturnDate", Value = QueryDate });
                sCondition["direct"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_direct.ArriveDate", Value = QueryDate });
            }

            string sSql = string.Format(@"
Select T.*
From
( 
    --收料单
    Select mms_receive.BillNo As SrcBillNo,'receive' As SrcBillType,mms_receive.ContractCode As ContractCode,
	    mms_receive.SupplierCode,mms_receive.OriginalNum,mms_receive.ReceiveDate As TakeDate,
	    mms_receiveDetail.MaterialCode,mms_receiveDetail.Unit,mms_receiveDetail.UnitPrice,
	    mms_receiveDetail.Num,mms_receiveDetail.Money,mms_receiveDetail.Remark,mms_merchants.MerchantsName AS SupplierName,
        '' as name,(mms_receive.BillNo+'receive'+mms_receiveDetail.MaterialCode) as id,mms_material.MaterialName,mms_material.Model
    From mms_receiveDetail
    Left join mms_receive ON mms_receiveDetail.BillNo=mms_receive.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_receive.SupplierCode
    left join mms_material on mms_material.MaterialCode=mms_receiveDetail.MaterialCode
    Where mms_receive.ApproveState='passed' {0}

    UNION

    --直入直出
    Select mms_direct.BillNo As SrcBillNo,'direct' As SrcBillType,mms_direct.ContractCode As ContractCode,
	    mms_direct.SupplierCode,mms_direct.OriginalNum,mms_direct.ArriveDate As TakeDate,
	    mms_directDetail.MaterialCode,mms_directDetail.Unit,mms_directDetail.UnitPrice,
	    mms_directDetail.Num,mms_directDetail.Money,mms_directDetail.Remark,mms_merchants.MerchantsName AS SupplierName,
        '' as name,(mms_direct.BillNo+'direct'+mms_directDetail.MaterialCode) as id,mms_material.MaterialName,mms_material.Model
    From mms_directDetail
    Left join mms_direct ON mms_directDetail.BillNo=mms_direct.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_direct.SupplierCode
    left join mms_material on mms_material.MaterialCode=mms_directDetail.MaterialCode
    Where mms_direct.ApproveState='passed' {1}

    UNION

    --退货
    Select mms_return.BillNo As SrcBillNo,'return' As SrcBillType,mms_return.ContractCode As ContractCode,
	    mms_return.SupplierCode,'' AS OriginalNum,mms_return.ReturnDate As TakeDate,
	    mms_returnDetail.MaterialCode,mms_returnDetail.Unit,mms_returnDetail.UnitPrice,
	    -mms_returnDetail.Num,-mms_returnDetail.Money,mms_returnDetail.Remark,mms_merchants.MerchantsName AS SupplierName,
        '' as name,(mms_return.BillNo+'return'+mms_returnDetail.MaterialCode) as id,mms_material.MaterialName,mms_material.Model
    From mms_returnDetail
    Left join mms_return ON mms_returnDetail.BillNo=mms_return.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_return.SupplierCode
    left join mms_material on mms_material.MaterialCode=mms_returnDetail.MaterialCode
    Where mms_return.ApproveState='passed' {2}

    UNION

    --租赁进场
    Select mms_rentIn.BillNo As SrcBillNo,'rentin' As SrcBillType,mms_rentIn.ContractCode As ContractCode,
	    mms_rentIn.SupplierCode,'' as OriginalNum,mms_rentIn.RentInDate As TakeDate,
	    mms_rentInDetail.MaterialCode,mms_rentInDetail.Unit,mms_rentInDetail.UnitPrice,
	    mms_rentInDetail.Num,'0' as Money,mms_rentInDetail.Remark,mms_merchants.MerchantsName AS SupplierName,
        '' as name,(mms_rentIn.BillNo+'rentIn'+mms_rentInDetail.MaterialCode) as id,mms_material.MaterialName,mms_material.Model
    From mms_rentInDetail
    Left join mms_rentIn ON mms_rentInDetail.BillNo=mms_rentIn.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_rentIn.SupplierCode
    left join mms_material on mms_material.MaterialCode=mms_rentInDetail.MaterialCode
    Where mms_rentIn.ApproveState='passed' {3}
) as T
Where 1=1 {4}
                ", sCondition["receive"], sCondition["direct"], sCondition["return"], sCondition["rentin"], sWhere);

            using (var db = Db.Context("Mms"))
            {
                result = db.Sql(sSql).QueryMany<dynamic>();
 
                var list = from q in result
                           group q by new { q.SupplierName, q.SupplierCode } into g
                            select new
                            {
                                name = g.Key.SupplierName,
                                id = g.Key.SupplierCode,
                                Money = g.Sum(x => (decimal?)x.Money),
                                SupplierCode = "",
                                UnitPrice="",
                                Num=""
                            };

                result.AddRange(list);
            }


            return result;
        }

        //发料情况分析（领料单位角度）
        public dynamic GetSendSummary(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();

            string PickUnitName = query["PickUnitName"] ?? string.Empty;
            string BuildPartCode = query["BuildPartCode"] ?? string.Empty;
            string SrcBillType = query["SrcBillType"] ?? string.Empty;
            string MaterialType = query["MaterialType"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string QueryDate = query["QueryDate"] ?? string.Empty;
            string sWhere = "";
            var result = new List<dynamic>();

            Dictionary<string, string> sCondition = new Dictionary<string, string>();
            sCondition.Add("send", string.Format(" And  mms_send.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("direct", string.Format(" And  mms_direct.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("refund", string.Format(" And  mms_refund.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("rentin", string.Format(" And  mms_rentin.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("lossReport", string.Format(" And  mms_lossReport.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("transfer", string.Format(" And  mms_transfer.ProjectCode='{0}'", CurrentProjectCode));

            if (PickUnitName.Length > 0)
            {
                sCondition["send"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["direct"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["refund"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["rentin"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["lossReport"] += string.Format(" And '其它'='{0}'", PickUnitName);
                sCondition["transfer"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
            }

            if (BuildPartCode.Length > 0)
            {
                sCondition["send"] += string.Format(" And mms_send.BuildPartCode='{0}'", BuildPartCode);
                sCondition["direct"] += string.Format(" And mms_direct.BuildPartCode='{0}'", BuildPartCode);
                sCondition["refund"] += string.Format(" And 1=0", BuildPartCode);
                sCondition["rentin"] += string.Format(" And mms_rentin.BuildPartCode='{0}'", BuildPartCode);
                sCondition["lossReport"] += string.Format(" And 1=0", PickUnitName);
                sCondition["transfer"] += string.Format(" And 1=0", PickUnitName);
            }

            if (MaterialName.Length > 0)
            {
                sCondition["send"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["direct"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["refund"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["rentin"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["lossReport"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["transfer"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            if (SrcBillType.Length > 0)
            {
                sWhere += string.Format(" And T.SrcBillType='{0}'", SrcBillType);
            }

            if (QueryDate.Length > 0)
            {
                sCondition["send"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_send.SendDate", Value = QueryDate });
                sCondition["direct"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_direct.ArriveDate", Value = QueryDate });
                sCondition["refund"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_refund.RefundDate", Value = QueryDate });
                sCondition["rentin"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_rentin.RentInDate", Value = QueryDate });
                sCondition["lossReport"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_lossReport.LossReportDate", Value = QueryDate });
                sCondition["transfer"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_transfer.TransferDate", Value = QueryDate });
            }

            string sSql = string.Format(@"
Select T.*
From
( 
	--发料单
    Select mms_send.BillNo As SrcBillNo,'send' As SrcBillType,mms_send.SendDate As TakeDate,
		mms_send.PickUnit,mms_merchants.MerchantsName as PickUnitName,mms_buildPart.BuildPartName,
	    mms_sendDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_sendDetail.Unit,mms_sendDetail.UnitPrice,mms_sendDetail.Num,mms_sendDetail.Money,
		mms_sendDetail.Remark,'' as name,(mms_send.BillNo+'send'+mms_sendDetail.MaterialCode) as id
    From mms_sendDetail
    Left join mms_send ON mms_sendDetail.BillNo=mms_send.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_send.PickUnit
    left join mms_material on mms_material.MaterialCode=mms_sendDetail.MaterialCode
	left join mms_buildPart on mms_buildPart.BuildPartCode=mms_send.BuildPartCode
    Where mms_send.ApproveState='passed' {0}

    UNION

    --直入直出
    Select mms_direct.BillNo As SrcBillNo,'direct' As SrcBillType,mms_direct.ArriveDate As TakeDate,
		mms_direct.PickUnit,mms_merchants.MerchantsName as PickUnitName,mms_buildPart.BuildPartName,
	    mms_directDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_directDetail.Unit,mms_directDetail.UnitPrice,mms_directDetail.Num,mms_directDetail.Money,
		mms_directDetail.Remark,'' as name,(mms_direct.BillNo+'direct'+mms_directDetail.MaterialCode) as id
    From mms_directDetail
    Left join mms_direct ON mms_directDetail.BillNo=mms_direct.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_direct.PickUnit
    left join mms_material on mms_material.MaterialCode=mms_directDetail.MaterialCode
	left join mms_buildPart on mms_buildPart.BuildPartCode=mms_direct.BuildPartCode
    Where mms_direct.ApproveState='passed' {1}

    UNION

    --退库
     Select mms_refund.BillNo As SrcBillNo,'refund' As SrcBillType,mms_refund.RefundDate As TakeDate,
		mms_refund.RefundUnit as PickUnit,mms_merchants.MerchantsName as PickUnitName,NULL AS BuildPartName,
	    mms_refundDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_refundDetail.Unit,mms_refundDetail.UnitPrice,-mms_refundDetail.Num,-mms_refundDetail.Money,
		mms_refundDetail.Remark,'' as name,(mms_refund.BillNo+'refund'+mms_refundDetail.MaterialCode) as id
    From mms_refundDetail
    Left join mms_refund ON mms_refundDetail.BillNo=mms_refund.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_refund.RefundUnit
    left join mms_material on mms_material.MaterialCode=mms_refundDetail.MaterialCode
    Where mms_refund.ApproveState='passed' {2}

    UNION

    --租赁进场
    Select mms_rentin.BillNo As SrcBillNo,'rentin' As SrcBillType,mms_rentin.RentInDate As TakeDate,
		mms_rentinDetail.UseUnit as PickUnit,mms_merchants.MerchantsName as PickUnitName,mms_buildPart.BuildPartName,
	    mms_rentinDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_rentinDetail.Unit,mms_rentinDetail.UnitPrice,mms_rentinDetail.Num,'0' As Money,
		mms_rentinDetail.Remark,'' as name,(mms_rentin.BillNo+'rentin'+mms_rentinDetail.MaterialCode) as id
    From mms_rentinDetail
    Left join mms_rentin ON mms_rentinDetail.BillNo=mms_rentin.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_rentinDetail.UseUnit
    left join mms_material on mms_material.MaterialCode=mms_rentinDetail.MaterialCode
	left join mms_buildPart on mms_buildPart.BuildPartCode=mms_rentin.BuildPartCode
    Where mms_rentin.ApproveState='passed' {3}

    UNION

	--报损
	Select mms_lossReport.BillNo As SrcBillNo,'lossReport' As SrcBillType,mms_lossReport.LossReportDate As TakeDate,
		'其他' as PickUnit,'其他' as PickUnitName,NULL AS BuildPartName,
	    mms_lossReportDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_lossReportDetail.Unit,mms_lossReportDetail.UnitPrice,mms_lossReportDetail.Num,mms_lossReportDetail.Money,
		mms_lossReportDetail.Remark,'' as name,(mms_lossReport.BillNo+'lossReport'+mms_lossReportDetail.MaterialCode) as id
    From mms_lossReportDetail
    Left join mms_lossReport ON mms_lossReportDetail.BillNo=mms_lossReport.BillNo
    left join mms_material on mms_material.MaterialCode=mms_lossReportDetail.MaterialCode
    Where mms_lossReport.ApproveState='passed' {4}

    UNION

	--调拨
	Select mms_transfer.BillNo As SrcBillNo,'transfer' As SrcBillType,mms_transfer.TransferDate As TakeDate,
		mms_transfer.ReceiveUnit as PickUnit,mms_merchants.MerchantsName as PickUnitName,NULL AS BuildPartName,
	    mms_transferDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,
		mms_transferDetail.Unit,mms_transferDetail.UnitPrice,mms_transferDetail.Num,mms_transferDetail.Money,
		mms_transferDetail.Remark,'' as name,(mms_transfer.BillNo+'transfer'+mms_transferDetail.MaterialCode) as id
    From mms_transferDetail
    Left join mms_transfer ON mms_transferDetail.BillNo=mms_transfer.BillNo
    left join mms_merchants on mms_merchants.MerchantsCode=mms_transfer.ReceiveUnit
    left join mms_material on mms_material.MaterialCode=mms_transferDetail.MaterialCode
    Where mms_transfer.ApproveState='passed' {5}

) as T
Where 1=1 {6}

                ", sCondition["send"], sCondition["direct"], sCondition["refund"], sCondition["rentin"], sCondition["lossReport"], sCondition["transfer"], sWhere);

            using (var db = Db.Context("Mms"))
            {
                result = db.Sql(sSql).QueryMany<dynamic>();
 
                var list = from q in result
                           group q by new { q.PickUnitName, q.PickUnit } into g
                           select new
                           {
                               name = g.Key.PickUnitName,
                               id = g.Key.PickUnit,
                               Money = g.Sum(x => (decimal?)x.Money),
                               PickUnit = "",
                               UnitPrice = "",
                               Num = ""
                           };

                result.AddRange(list);
            }


            return result;
        }

        //库存查询
        public dynamic GetWarehouseQuery(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();

            string WarehouseCode = query["WarehouseCode"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string sWhere = "";
            var result = new List<dynamic>();

            if (WarehouseCode.Length > 0)
            {
                sWhere += string.Format(" And mms_warehouse.WarehouseCode='{0}'", WarehouseCode);
            }

            if (MaterialName.Length > 0)
            {
                sWhere += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            string sSql = string.Format(@"
Select mms_warehouseStock.*,mms_material.MaterialName,mms_material.Model,mms_warehouse.WarehouseName,
    mms_warehouseStock.WarehouseCode+'_'+mms_material.MaterialCode as id,'' AS name
From mms_warehouseStock
Left join mms_warehouse on mms_warehouseStock.WarehouseCode=mms_warehouse.WarehouseCode
Left join mms_material on mms_material.MaterialCode=mms_warehouseStock.MaterialCode
Where mms_warehouse.ProjectCode='{0}' {1}

                ", CurrentProjectCode, sWhere);

            using (var db = Db.Context("Mms"))
            {
                result = db.Sql(sSql).QueryMany<dynamic>();
     
                var list = from q in result
                           group q by new { q.WarehouseName, q.WarehouseCode } into g
                           select new
                           {
                               name = g.Key.WarehouseName,
                               id = g.Key.WarehouseCode,
                               Money = g.Sum(x => (decimal?)x.Money),
                               UnitPrice = "",
                               Num = ""
                           };

                result.AddRange(list);
            }

            return result;
        }

        //库存流水查询
        public dynamic GetWarehouseAccount(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();

            string MaterialType = query["MaterialType"] ?? string.Empty;
            string CreatePerson = query["CreatePerson"] ?? string.Empty;
            string SrcBillType = query["SrcBillType"] ?? string.Empty;
            string SrcBillNo = query["SrcBillNo"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string QueryDate = query["QueryDate"] ?? string.Empty;
            string  sWhere = "";
            string order = query["order"] ?? string.Empty;
            int page = ZConvert.To<int>(query["page"], 1);
            int rows = ZConvert.To<int>(query["rows"], 0);

            if (CreatePerson.Length > 0)
            {
                sWhere += string.Format(" And mms_warehouseStockHistory.CreatePerson like '%{0}%'", CreatePerson);
            }

            if (MaterialName.Length > 0)
            {
                sWhere += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            if (SrcBillType.Length > 0)
            {
                sWhere += string.Format(" And mms_warehouseStockHistory.SrcBillType='{0}'", SrcBillType);
            }

            if (SrcBillNo.Length > 0)
            {
                sWhere += string.Format(" And mms_warehouseStockHistory.SrcBillNo like '%{0}%'", SrcBillNo);
            }

            if (QueryDate.Length > 0)
            {
                sWhere += " And " + Cp.DateRange(new WhereData() { Column = "mms_warehouseStockHistory.CreateDate", Value = QueryDate });
            }

            string sSql = string.Format(@"
Select mms_warehouseStockHistory.*,mms_material.MaterialName,mms_material.Model,mms_material.Unit
From mms_warehouseStockHistory
Left join mms_warehouse on mms_warehouse.WarehouseCode=mms_warehouseStockHistory.WarehouseCode
Left join mms_material on mms_material.MaterialCode=mms_warehouseStockHistory.MaterialCode
Where  mms_warehouse.ProjectCode='{0}' {1}
order by mms_warehouseStockHistory.CreateDate desc
                ", CurrentProjectCode, sWhere);


            dynamic result = new ExpandoObject();
            using (var db = Db.Context("Mms"))
            {
                var data = db.Sql(sSql).QueryMany<dynamic>();
                result.rows = data.Skip((page-1)*rows).Take(rows);
                result.total = data.Count();
            }
 
            return result;
        }

        //材料预警提示
        public dynamic GetMaterialWarnning(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();

            string MaterialType = query["MaterialType"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string IsWarnning = query["IsWarnning"] ?? string.Empty;
            string sWhere = "";
            string order = query["order"] ?? string.Empty;
            int page = ZConvert.To<int>(query["page"], 1);
            int rows = ZConvert.To<int>(query["rows"], 0);

            if (MaterialName.Length > 0)
            {
                sWhere += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            if (IsWarnning == "true")
            {
                sWhere += string.Format(" And (mms_materialWarnning.LowerNum>mms_materialWarnning.Num OR mms_materialWarnning.Num>mms_materialWarnning.UpperNum)");
            }

            string sSql = string.Format(@"
Select mms_materialWarnning.*,mms_material.MaterialName,mms_material.Model,mms_material.Unit
From mms_materialWarnning
Left join mms_material on mms_material.MaterialCode=mms_materialWarnning.MaterialCode
Where mms_materialWarnning.ProjectCode='{0}' {1}
                ", CurrentProjectCode, sWhere);

            dynamic result = new ExpandoObject();
            using (var db = Db.Context("Mms"))
            {
                var data = db.Sql(sSql).QueryMany<dynamic>();
                result.rows = data.Skip((page - 1) * rows).Take(rows);
                result.total = data.Count();
            }

            return result;
        }

        //单击取得材料库房分布明细
        public dynamic GetMaterialDetail(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();
            string MaterialCode = query["MaterialCode"] ?? string.Empty;

            var result = new List<dynamic>();
            
            string sSql = string.Format(@"
Select *
From mms_warehouseStock
Left join mms_warehouse on mms_warehouse.WarehouseCode=mms_warehouseStock.WarehouseCode
Where mms_warehouse.ProjectCode='{0}' and mms_warehouseStock.MaterialCode='{1}'
                ", CurrentProjectCode, MaterialCode);

            using (var db = Db.Context("Mms"))
            {
                result = db.Sql(sSql).QueryMany<dynamic>();
            }

            return result;
        }
 
        //收发-结存报表
        public dynamic GetBalanceAccount(RequestWrapper query)
        {
            var CurrentProjectCode = MmsHelper.GetCurrentProject();
            string WarehouseCode = query["WarehouseCode"] ?? string.Empty;
            string BuildPartCode = query["BuildPartCode"] ?? string.Empty;
            string PickUnitName = query["PickUnitName"] ?? string.Empty;
            string MaterialName = query["MaterialName"] ?? string.Empty;
            string QueryDate = query["QueryDate"] ?? string.Empty;
            string sortField = query["sort"] ?? "MaterialName";
            string order = query["order"] ?? string.Empty;
            int page = ZConvert.To<int>(query["page"], 1);
            int rows = ZConvert.To<int>(query["rows"], 0);

            string sTitle = "";
            var result = new List<dynamic>();
            var initResult = new List<dynamic>();

            #region 条件
            Dictionary<string, string> sCondition = new Dictionary<string, string>();
            sCondition.Add("receive", string.Format(" And  mms_receive.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("return", string.Format(" And  mms_return.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("direct", string.Format(" And  mms_direct.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("rentin", string.Format(" And  mms_rentin.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("adjust", string.Format(" And  mms_stockAdjust.ProjectCode='{0}'", CurrentProjectCode));

            sCondition.Add("send", string.Format(" And  mms_send.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("refund", string.Format(" And  mms_refund.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("transfer", string.Format(" And  mms_transfer.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("lossReport", string.Format(" And  mms_lossReport.ProjectCode='{0}'", CurrentProjectCode));
            sCondition.Add("init", string.Format(" And  mms_warehouse.ProjectCode='{0}'", CurrentProjectCode));

            if (WarehouseCode.Length > 0)
            {
                sCondition["receive"] += string.Format(" And mms_receive.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["return"] += string.Format(" And mms_return.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["direct"] += string.Format(" And 1=0");
                sCondition["rentin"] += string.Format(" And 1=0");
                sCondition["adjust"] += string.Format(" And mms_stockAdjust.WarehouseCode ='{0}'", WarehouseCode);

                sCondition["send"] += string.Format(" And mms_send.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["refund"] += string.Format(" And mms_refund.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["transfer"] += string.Format(" And mms_transfer.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["lossReport"] += string.Format(" And mms_lossReport.WarehouseCode ='{0}'", WarehouseCode);
                sCondition["init"] += string.Format(" And mms_warehouseStockHistory.WarehouseCode ='{0}'", WarehouseCode);
            }

            if (BuildPartCode.Length > 0)
            {
                sCondition["receive"] += string.Format(" And 1=0");
                sCondition["return"] += string.Format(" And 1=0");
                sCondition["direct"] += string.Format(" And mms_direct.BuildPartCode ='{0}'", BuildPartCode);
                sCondition["rentin"] += string.Format(" And mms_rentin.BuildPartCode ='{0}'", BuildPartCode);
                sCondition["adjust"] += string.Format(" And 1=0");

                sCondition["send"] += string.Format(" And mms_send.BuildPartCode ='{0}'", BuildPartCode);
                sCondition["refund"] += string.Format(" And 1=0");
                sCondition["transfer"] += string.Format(" And 1=0");
                sCondition["lossReport"] += string.Format(" And 1=0");
            }

            if (PickUnitName.Length > 0)
            {
                sCondition["receive"] += string.Format(" And 1=0");
                sCondition["return"] += string.Format(" And 1=0");
                sCondition["direct"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["rentin"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["adjust"] += string.Format(" And 1=0");

                sCondition["send"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["refund"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["transfer"] += string.Format(" And mms_merchants.MerchantsName like '%{0}%'", PickUnitName);
                sCondition["lossReport"] += string.Format(" And 1=0");
            }

            if (MaterialName.Length > 0)
            {
                sCondition["receive"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["return"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["direct"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["rentin"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["adjust"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);

                sCondition["send"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["refund"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["transfer"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["lossReport"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
                sCondition["init"] += string.Format(" And mms_material.MaterialName like '%{0}%'", MaterialName);
            }

            if (QueryDate.Length > 0)
            {
                sCondition["receive"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_receive.ReceiveDate", Value = QueryDate });
                sCondition["return"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_return.ReturnDate", Value = QueryDate });
                sCondition["direct"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_direct.ArriveDate", Value = QueryDate });
                sCondition["rentin"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_rentin.RentinDate", Value = QueryDate });
                sCondition["adjust"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_stockAdjust.EffectDate", Value = QueryDate });

                sCondition["send"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_send.SendDate", Value = QueryDate });
                sCondition["refund"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_refund.RefundDate", Value = QueryDate });
                sCondition["transfer"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_transfer.TransferDate", Value = QueryDate });
                sCondition["lossReport"] += " and " + Cp.DateRange(new WhereData() { Column = "mms_lossReport.LossReportDate", Value = QueryDate });
                sCondition["init"] += " and mms_warehouseStockHistory.CreateDate<'" + QueryDate.Split('到')[0].Trim(' ') + "'" ;
            }
            #endregion

            //1.取得数据源的Sql
            string sqlSource = GetTargetData(sCondition);

            //2.取得发料的领料单位列表
            string sqlPickList = string.Format(@"
Select PickUnitName
From ({0}) as t
Where PickUnitName<>'' and PickUnitName<>'其他' and PickUnitName<>'租赁进场'
group by PickUnitName
                ", sqlSource);

            using (var db = Db.Context("Mms"))
            {
                //2.取得领料单位数据
                string sDynamicColumn = "";
                var returnPickList = db.Sql(sqlPickList).QueryMany<dynamic>();

                for (int i = 0; i < returnPickList.Count; i++)
                {
                    string sPickUnitName = returnPickList[i].PickUnitName;
                    sDynamicColumn += string.Format(" ,ISNULL(Sum(CASE WHEN PickUnitName='{0}' then Num else 0 end),0) as PickUnitNum{1}", sPickUnitName, i);
                    sDynamicColumn += string.Format(" ,ISNULL(Sum(CASE WHEN PickUnitName='{0}' then Money else 0 end),0) as PickUnitMoney{1}", sPickUnitName, i);
                }

                //3.取得外层Sql
                string sql = string.Format(GetBalanceSql(), sDynamicColumn, sqlSource);

                result = db.Sql(sql).QueryMany<dynamic>();

                sTitle = GetBanlanceTitle(returnPickList);

                //4.从履历表里取得期初的数据
                sql = string.Format(@"
Select mms_warehouseStockHistory.MaterialCode,sum(Num) As InitNum,sum(UnitPrice*Num) As InitMoney
From mms_warehouseStockHistory
Left join mms_warehouse on mms_warehouse.warehouseCode=mms_warehouseStockHistory.warehouseCode
Left join mms_material on mms_material.MaterialCode=mms_warehouseStockHistory.MaterialCode
Where 1=1 {0}
group by mms_warehouseStockHistory.MaterialCode
"
                    , sCondition["init"]);

                initResult = db.Sql(sql).QueryMany<dynamic>();

            }

            var list = from q in result
                       join p in initResult on q.MaterialCode equals p.MaterialCode into temp
                       from tt in temp.DefaultIfEmpty()

                       select Extend(q, new
                       {
                           InitNum = tt == null ? 0 : tt.InitNum,
                           InitMoney = tt == null ? 0 : tt.InitMoney,
                           TotalMoney = (tt == null ? 0 : tt.InitMoney) + ((q.ReceiveMoney == null) ? 0 : q.ReceiveMoney) - ((q.SendMoney == null) ? 0 : q.SendMoney),
                           TotalNum = (tt == null ? 0 : tt.InitNum) + ((q.ReceiveNum == null) ? 0 : q.ReceiveNum) - ((q.SendNum == null) ? 0 : q.SendNum)
                       });
                    
            if(order == "desc")
                list = list.OrderByDescending(x => ((IDictionary<string, object>)x)[sortField]).Skip((page - 1) * rows).Take(rows);
            else
                list = list.OrderBy(x => ((IDictionary<string, object>)x)[sortField]).Skip((page-1) * rows).Take(rows);

            dynamic rtnData = new ExpandoObject();
            rtnData.Title = sTitle;
            rtnData.rows = list.ToList();
            return rtnData;
        }

        private dynamic Extend(object q, object newObj)
        {
            var result = (IDictionary<string, object>)q;
            EachHelper.EachObjectProperty(newObj, (i, name, value) => result[name] = value );
            return result;
        }
        
        //收发-结存报表的Sql
        private string GetTargetData(Dictionary<string, string> sCondition)
        {
            //材料名称，材料编码，型号，单位，期初，收料，发料，结存
            string sSqlDetail = string.Format(@"
--收料单
Select mms_receiveDetail.BillNo As SrcBillNo,mms_receiveDetail.RowId As SrcRowId,'receive' As SrcBillType,mms_receive.ReceiveDate As TakeDate,
	mms_receive.SupplyType As SupplyType,'' As PickUnitName, mms_receiveDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_receiveDetail.Unit,
	mms_receiveDetail.UnitPrice,mms_receiveDetail.Num,mms_receiveDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_receive
Left join mms_receiveDetail On mms_receiveDetail.BillNo=mms_receive.BillNo
Left join mms_material On mms_material.MaterialCode=mms_receiveDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Where mms_receive.ApproveState='passed' {0}

Union
--退货单
Select mms_returnDetail.BillNo As SrcBillNo,mms_returnDetail.RowId As SrcRowId,'return' As SrcBillType,mms_return.ReturnDate As TakeDate,
	'其他' As SupplyType,'' As PickUnitName, mms_returnDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_returnDetail.Unit,
	mms_returnDetail.UnitPrice,-mms_returnDetail.Num,-mms_returnDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_return
Left join mms_returnDetail On mms_returnDetail.BillNo=mms_return.BillNo
Left join mms_material On mms_material.MaterialCode=mms_returnDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Where mms_return.ApproveState='passed' {1}

Union
--直入直出
Select mms_directDetail.BillNo As SrcBillNo,mms_directDetail.RowId As SrcRowId,'direct' As SrcBillType,mms_direct.ArriveDate As TakeDate,
	mms_direct.SupplyType As SupplyType,mms_merchants.MerchantsName As PickUnitName, mms_directDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_directDetail.Unit,
	mms_directDetail.UnitPrice,mms_directDetail.Num,mms_directDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_direct
Left join mms_directDetail On mms_directDetail.BillNo=mms_direct.BillNo
Left join mms_material On mms_material.MaterialCode=mms_directDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Left join mms_merchants On mms_merchants.MerchantsCode=mms_direct.PickUnit
Where mms_direct.ApproveState='passed' {2}

Union
--租赁进场
Select mms_rentinDetail.BillNo As SrcBillNo,mms_rentinDetail.RowId As SrcRowId,'rentin' As SrcBillType,mms_rentin.RentInDate As TakeDate,
	'租赁进场' As SupplyType,'租赁进场' As PickUnitName, mms_rentinDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_rentinDetail.Unit,
	mms_rentinDetail.UnitPrice,mms_rentinDetail.Num,'0' As Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_rentin
Left join mms_rentinDetail On mms_rentinDetail.BillNo=mms_rentin.BillNo
Left join mms_material On mms_material.MaterialCode=mms_rentinDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Left join mms_merchants On mms_merchants.MerchantsCode=mms_rentinDetail.UseUnit
Where mms_rentin.ApproveState='passed' {3}

Union
--库存调整
Select mms_stockAdjustDetail.BillNo As SrcBillNo,mms_stockAdjustDetail.RowId As SrcRowId,'adjust' As SrcBillType,mms_stockAdjust.EffectDate As TakeDate,
	'其他' As SupplyType,'' As PickUnitName, mms_stockAdjustDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_stockAdjustDetail.Unit,
	mms_stockAdjustDetail.UnitPrice,mms_stockAdjustDetail.Num,mms_stockAdjustDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_stockAdjust
Left join mms_stockAdjustDetail On mms_stockAdjustDetail.BillNo=mms_stockAdjust.BillNo
Left join mms_material On mms_material.MaterialCode=mms_stockAdjustDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Where mms_stockAdjust.ApproveState='passed' {4}

Union
--发料
Select mms_sendDetail.BillNo As SrcBillNo,mms_sendDetail.RowId As SrcRowId,'send' As SrcBillType,mms_send.SendDate As TakeDate,
	'' As SupplyType, mms_merchants.MerchantsName As PickUnitName,mms_sendDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_sendDetail.Unit,
	mms_sendDetail.UnitPrice,mms_sendDetail.Num,mms_sendDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_send
Left join mms_sendDetail On mms_sendDetail.BillNo=mms_send.BillNo
Left join mms_material On mms_material.MaterialCode=mms_sendDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Left join mms_merchants On mms_merchants.MerchantsCode=mms_send.PickUnit
Where mms_send.ApproveState='passed' {5}

Union
--退库
Select mms_refundDetail.BillNo As SrcBillNo,mms_refundDetail.RowId As SrcRowId,'refund' As SrcBillType,mms_refund.RefundDate As TakeDate,
	'' As SupplyType, mms_merchants.MerchantsName As PickUnitName,mms_refundDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_refundDetail.Unit,
	mms_refundDetail.UnitPrice,-mms_refundDetail.Num,-mms_refundDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_refund
Left join mms_refundDetail On mms_refundDetail.BillNo=mms_refund.BillNo
Left join mms_material On mms_material.MaterialCode=mms_refundDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Left join mms_merchants On mms_merchants.MerchantsCode=mms_refund.RefundUnit
Where mms_refund.ApproveState='passed' {6}

Union
--调拨
Select mms_transferDetail.BillNo As SrcBillNo,mms_transferDetail.RowId As SrcRowId,'transfer' As SrcBillType,mms_transfer.TransferDate As TakeDate,
	'' As SupplyType, mms_merchants.MerchantsName As PickUnitName,mms_transferDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_transferDetail.Unit,
	mms_transferDetail.UnitPrice,mms_transferDetail.Num,mms_transferDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_transfer
Left join mms_transferDetail On mms_transferDetail.BillNo=mms_transfer.BillNo
Left join mms_material On mms_material.MaterialCode=mms_transferDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Left join mms_merchants On mms_merchants.MerchantsCode=mms_transfer.ReceiveUnit
Where mms_transfer.ApproveState='passed'{7}

Union
--报损
Select mms_lossReportDetail.BillNo As SrcBillNo,mms_lossReportDetail.RowId As SrcRowId,'lossReport' As SrcBillType,mms_lossReport.LossReportDate As TakeDate,
	'' As SupplyType, '其他' As PickUnitName,mms_lossReportDetail.MaterialCode,mms_material.MaterialName,mms_material.Model,mms_lossReportDetail.Unit,
	mms_lossReportDetail.UnitPrice,mms_lossReportDetail.Num,mms_lossReportDetail.Money,mms_material.MaterialType,mms_materialType.Type As MaterialKind
From mms_lossReport
Left join mms_lossReportDetail On mms_lossReportDetail.BillNo=mms_lossReport.BillNo
Left join mms_material On mms_material.MaterialCode=mms_lossReportDetail.MaterialCode
Left join mms_materialType On mms_materialType.MaterialType=mms_material.MaterialType
Where mms_lossReport.ApproveState='passed' {8}
"
                , sCondition["receive"], sCondition["return"], sCondition["direct"], sCondition["rentin"], sCondition["adjust"]
                , sCondition["send"], sCondition["refund"], sCondition["transfer"], sCondition["lossReport"]);

            return sSqlDetail;
        }

        private string GetBalanceSql()
        {
            string sSql = @"
Select MaterialCode,MaterialName,Model,MaterialKind,MaterialType,Unit
    ,ISNULL(Sum(CASE WHEN SupplyType<>'' then Money else 0 end),0) as ReceiveMoney --合计
    ,ISNULL(Sum(CASE WHEN SupplyType='082' then Money else 0 end),0) as StockSelfMoney --自采
    ,ISNULL(Sum(CASE WHEN SupplyType='083' then Money else 0 end),0) as OwnerSupplyMoney --甲供
    ,ISNULL(Sum(CASE WHEN SupplyType='084' then Money else 0 end),0) as CallInMoney --调入
    ,ISNULL(Sum(CASE WHEN SupplyType='其他' then Money else 0 end),0) as ReceiveOtherMoney --其他

    ,ISNULL(Sum(CASE WHEN SupplyType<>'' then Num else 0 end),0) as ReceiveNum --合计
    ,ISNULL(Sum(CASE WHEN SupplyType='082' then Num else 0 end),0) as StockSelfNum --自采
    ,ISNULL(Sum(CASE WHEN SupplyType='083' then Num else 0 end),0) as OwnerSupplyNum --甲供
    ,ISNULL(Sum(CASE WHEN SupplyType='084' then Num else 0 end),0) as CallInNum --调入
	,ISNULL(Sum(CASE WHEN SupplyType='租赁进场' then Num else 0 end),0) as ReceiveRentInNum --租赁进场
    ,ISNULL(Sum(CASE WHEN SupplyType='其他' then Num else 0 end),0) as ReceiveOtherNum --其他

    ,ISNULL(Sum(CASE WHEN PickUnitName<>'' then Money else 0 end),0) as SendMoney --合计
    ,ISNULL(Sum(CASE WHEN PickUnitName<>'' then Num else 0 end),0) as SendNum --合计
    {0}                                                             --领料人生成的列
    ,ISNULL(Sum(CASE WHEN PickUnitName='租赁进场' then Num else 0 end),0) as SendRentInNum --租赁进场
    ,ISNULL(Sum(CASE WHEN PickUnitName='其他' then Money else 0 end),0) as SendOtherMoney --其他
    ,ISNULL(Sum(CASE WHEN PickUnitName='其他' then Num else 0 end),0) as SendOtherNum --其他

    ,ISNULL(Sum(CASE WHEN SupplyType<>'' then Num else -Num end),0) as TotalNum --合计
    ,ISNULL(Sum(CASE WHEN SupplyType<>'' then Money else -Money end),0) as TotalMoney --合计

From ({1}) as t
group by MaterialCode,MaterialName,Model,MaterialKind,MaterialType,Unit
";

            return sSql;
        }

        private string GetBanlanceTitle(dynamic PickList)
        {
            string sPickTitle = "";
            string sPickField = "";
            string PickTitleModel = @"{{""title"":""{0}"",""colspan"":2}},";
            string PickFieldModel = @"
{{""field"":""PickUnitNum{0}"",""title"":""数量"",""width"":50,""align"":""right""}},
{{""field"":""PickUnitMoney{0}"",""title"":""金额"",""width"":60,""align"":""right""}},";

            for (int i = 0; i < PickList.Count; i++)
            {
                int length = Math.Min(8,PickList[i].PickUnitName.Length);
                sPickTitle += string.Format(PickTitleModel, PickList[i].PickUnitName.Substring(0, length));
                sPickField += string.Format(PickFieldModel, i);
            }

            string sGridColumns = string.Format(@"
[
    [
        {{ ""title"":""期初结存"",""rowspan"":2,""colspan"":2}},
        {{ ""title"":""收料"",""colspan"":11}},
        {{ ""title"":""发料"",""colspan"":{2}}},
        {{ ""title"":""结存"",""colspan"":2,""rowspan"":2}}
    ],
    [ 
        {{""title"":""合计"",""colspan"":2}},
        {{""title"":""自采"",""colspan"":2}},
        {{""title"":""甲供"",""colspan"":2}},
        {{""title"":""调入"",""colspan"":2}},
        {{""title"":""租赁进场"",""width"":60}},
        {{""title"":""其他"",""colspan"":2}},
                 
        {{""title"":""合计"",""colspan"":2}},
        {0}
        {{""title"":""租赁进场"",""width"":50}},
        {{""title"":""其他"",""colspan"":2}}
    ],
    [ 
        {{""field"":""InitNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""InitMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
                
        {{""field"":""ReceiveNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""ReceiveMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
        {{""field"":""StockSelfNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""StockSelfMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
        {{""field"":""OwnerSupplyNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""OwnerSupplyMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
        {{""field"":""CallInNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""CallInMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
        {{""field"":""ReceiveRentInNum"",""title"":""数量"",""width"":60,""rowspan"":1,""colspan"":1,""align"":""right""}},
        {{""field"":""ReceiveOtherNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""ReceiveOtherMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
                 
        {{""field"":""SendNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""SendMoney"",""title"":""金额"",""width"":60,""align"":""right""}},
        {1}
        {{""field"":""SendRentInNum"",""title"":""数量"",""width"":60,""rowspan"":1,""colspan"":1,""align"":""right""}},
        {{""field"":""SendOtherNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""SendOtherMoney"",""title"":""金额"",""width"":60,""align"":""right""}},

        {{""field"":""TotalNum"",""title"":""数量"",""width"":50,""align"":""right""}},
        {{""field"":""TotalMoney"",""title"":""金额"",""width"":60,""align"":""right""}}
    ]
]
"
                , sPickTitle, sPickField, PickList.Count*2 + 5);
            
            return sGridColumns;

        }
    }
}