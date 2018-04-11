using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Data;
using Zephyr.Models;

namespace Zephyr.Web.Areas.Mms.Common
{
    [Module("Mms")]
    public class MmsService : ServiceBase<ModelBase>
    {
        //审核表单
        public int Audit(string tableName, string billNo, string status, string comment)
        {
            db.UseTransaction(true);

            var userName = MmsHelper.GetUserName();
            var pUpdate = ParamUpdate.Instance()
                .Update(tableName)
                .Column("ApproveState", status)
                .Column("ApproveRemark", comment)
                .Column("ApprovePerson", userName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("BillNo", billNo);

            var rowsAffected = BuilderParse(pUpdate).Execute();

            if (rowsAffected<=0)
            {
                db.Rollback();
                return rowsAffected;
            }

            switch (tableName)
            {
                case "mms_receive":
                case "mms_refund":
                    rowsAffected = mms_warehouseStockService.UpdateWarehouseStock(db, tableName, billNo, status == "passed");
                    break;

                case "mms_send":
                case "mms_return":
                case "mms_transfer":
                case "mms_lossReport":
                    rowsAffected = mms_warehouseStockService.UpdateWarehouseStock(db, tableName, billNo, status != "passed");
                    break;

                case "mms_rentOut":
                    rowsAffected = mms_rentOutService.CalcRentOutMoney(db, billNo);
                    break;
            }
 
            if (rowsAffected<0)
            {
                db.Rollback();
                return rowsAffected;
            }

            db.Commit();
            return rowsAffected;
        }

        public static void LoginHandler(JObject request) 
        {
            var defaultProject = "201306030001";
            var projectName = (new mms_projectService().GetModel(ParamQuery.Instance().AndWhere("ProjectCode", defaultProject))??new mms_project()).ProjectName;
            var cookieProjectCode = new HttpCookie("CurrentProject") { Value = defaultProject,Expires = DateTime.MaxValue};
            var cookieProjectName = new HttpCookie("CurrentProjectName") { Value = HttpUtility.UrlEncode(projectName), Expires = DateTime.MaxValue };

            var response = HttpContext.Current.Response;
            response.Cookies.Add(cookieProjectCode);
            response.Cookies.Add(cookieProjectName);
        }

        public static List<dynamic> GetReceiveSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "receive", text = "收料单" });
            result.Add(new { value = "return", text = "退货单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });

            return result;
        }

        public static List<dynamic> GetSendSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "send", text = "发料单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "refund", text = "退库单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });
            result.Add(new { value = "lossReport", text = "报损单" });
            result.Add(new { value = "transfer", text = "调拨单" });

            return result;
        }

        public static List<dynamic> GetAccountSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "receive", text = "收料单" });
            result.Add(new { value = "send", text = "发料单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "refund", text = "退库单" });
            result.Add(new { value = "return", text = "退货单" });
            result.Add(new { value = "lossReport", text = "报损单" });
            result.Add(new { value = "transfer", text = "调拨单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });

            return result;
        }


        //生成批次信息及更新来源单剩余数量
        public static void HandlerBillBatchesAfterEdit<TBill>(EditEventArgs arg)
        {
            #region 变量定义
            var billTable = typeof(TBill).Name;
            var billNo = arg.form["BillNo"].ToString();
            var userName = MmsHelper.GetUserName();
            var form = arg.db.Sql(String.Format("select * from {0} where BillNo='{1}'", billTable, billNo)).QuerySingle<dynamic>();

            var isBatch = billTable=="mms_lossReport" || form.PriceKind == "091";
            var table = new Dictionary<string, string>{ 
                {"receive","mms_receiveDetail"},
                {"refund","mms_refundDetail"},
                {"adjust","mms_stockAdjustDetail"}
            };
            #endregion

            #region 把原单据剩余数量加回去并且删除原批次信息
            var listOld = arg.db.Select<mms_sendBatches>("*")
                .From(string.Format("{0}Batches", billTable))
                .Where("BillNo=@0").Parameters(billNo)
                .QueryMany();

            foreach (var  batch in listOld)
            {
                string tableName = table[batch.SrcBillType];
                arg.db.Sql(string.Format(@"
update {0} 
set RemainNum = isnull(RemainNum,0) + {1} ,
UpdateDate = getdate(),
UpdatePerson = '{2}'
where BillNo='{3}' 
and RowId='{4}'", tableName, batch.Num, userName, batch.SrcBillNo, batch.SrcRowId)).Execute();
            }

            arg.db.Delete(String.Format("{0}Batches", billTable)).Where("BillNo", billNo).Execute();
            #endregion

            #region 重新生成批次信息
            var rows = arg.db.Select<mms_sendDetail>("*")
                .From(String.Format("{0}Detail", billTable))
                .AndWhere("BillNo=@0").Parameters(billNo)
                .QueryMany();

            foreach (var row in rows)
            {
                var pQuery = RequestWrapper.Instance()
                        .LoadSettingXml("~/Areas/Mms/Views/Shared/Xml/material_batches.xml")
                        .ToParamQuery();

                if (isBatch)
                {
                    #region 按批次计价
                    var batch = new mms_sendBatches()
                    {
                        BillNo = row.BillNo,
                        RowId = row.RowId,
                        MaterialCode = row.MaterialCode,
                        Money = row.Money,
                        Num = row.Num,
                        SrcBillType = row.SrcBillType,
                        SrcBillNo = row.SrcBillNo,
                        SrcRowId = row.SrcRowId,
                        Remark = row.Remark,
                        CreateDate = row.CreateDate,
                        CreatePerson = row.CreatePerson,
                        UpdateDate = row.UpdateDate,
                        UpdatePerson = row.UpdatePerson
                    };
                    arg.db.Insert<mms_sendBatches>(string.Format("{0}Batches", billTable), batch).AutoMap().Execute();

                    string sql = @"select * from "+ pQuery.GetData().From +String.Format(@"
where WarehouseCode='{0}'
and MaterialCode='{1}'
and SrcBillType = '{2}'
and SrcBillNo = '{3}'
and SrcRowId ='{4}' 
order by SrcDate ", form.WarehouseCode, row.MaterialCode, batch.SrcBillType, batch.SrcBillNo, batch.SrcRowId);

                    var item = arg.db.Sql(sql).QuerySingle<dynamic>();

                    if (item == null)
                        throw new Exception("批次材料找不到来源单，可能已被删除！不能修改！");

                    if (batch.Num > item.RemainNum)
                        throw new Exception("按批次材料数量不足，请刷新后重试！");

                    //更新来源单剩余数量
                    string tableName = table[batch.SrcBillType];
                    arg.db.Sql(string.Format(@"
update {0} 
set RemainNum = isnull(RemainNum,0) - {1} ,
UpdateDate = getdate(),
UpdatePerson = '{2}'
where BillNo='{3}' 
and RowId='{4}'
", tableName, batch.Num, userName, batch.SrcBillNo, batch.SrcRowId)).Execute();
                    #endregion
                }
                else
                {
                    #region 按先进先出
                    decimal qty = row.Num ?? 0;
                    string sql = @"select * from "+ pQuery.GetData().From + 
string.Format(@"where WarehouseCode='{0}' and MaterialCode='{1}' order by SrcDate " 
,form.WarehouseCode,row.MaterialCode);

                    var list = arg.db.Sql(sql).QueryMany<dynamic>();

                    foreach (var item in list)
                    {
                        string tableName = table[item.SrcBillType];
                        decimal srcRemainQty = item.RemainNum;
                        decimal handleQty = qty <= srcRemainQty ? qty : srcRemainQty;

                        var batch = new mms_sendBatches()
                        {
                            BillNo = row.BillNo,
                            RowId = int.Parse(NewKey.maxplus(arg.db, tableName, "RowId", ParamQuery.Instance().AndWhere("BillNo", row.BillNo))),
                            MaterialCode = row.MaterialCode,
                            Money = row.Money,
                            Num = handleQty,
                            SrcBillType = item.SrcBillType,
                            SrcBillNo = item.SrcBillNo,
                            SrcRowId = item.SrcRowId,
                            Remark = row.Remark,
                            CreateDate = DateTime.Now,
                            CreatePerson = userName,
                            UpdateDate = DateTime.Now,
                            UpdatePerson = userName
                        };

                        //生成批次信息
                        arg.db.Insert<mms_sendBatches>(string.Format("{0}Batches", billTable), batch).AutoMap().Execute();

                        //更新来源单剩余数量
                        arg.db.Sql(string.Format(@"
update {0} 
set RemainNum = isnull(RemainNum,0) - {1} ,
UpdateDate = getdate(),
UpdatePerson = '{2}'
where BillNo='{3}' 
and RowId='{4}'", tableName, batch.Num, userName, batch.SrcBillNo, batch.SrcRowId)).Execute();

                        qty = qty - handleQty;
                        if (qty <= 0) break;
                    }

                    if (qty > 0)
                        throw new Exception("批次中的材料数量不足，请刷新后重试！");
                    #endregion
                }


            }
            #endregion

      
        }
    }
}