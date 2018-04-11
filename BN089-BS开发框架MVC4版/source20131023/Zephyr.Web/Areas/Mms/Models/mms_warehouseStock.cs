using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Data;
using Zephyr.Utils;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_warehouseStockService : ServiceBase<mms_warehouseStock>
    {
        //更新材料库存信息
        public static int UpdateWarehouseStock(IDbContext context, string warehouseCode, string MaterialCode,string Unit,decimal UnitPrice, decimal Num)
        {
            var userName = MmsHelper.GetUserName();

            var builder = context.Select<mms_warehouseStock>("*")
                   .From("mms_warehouseStock")
                   .Where("WarehouseCode=@0 and MaterialCode=@1")
                   .Parameters(warehouseCode, MaterialCode);

            mms_warehouseStock stock = builder.QuerySingle() ?? new mms_warehouseStock()
            {
                WarehouseCode = warehouseCode,
                MaterialCode = MaterialCode,
                Unit = Unit,
                UnitPrice = 0,
                Num = 0,
                Money = 0,
                CreateDate = DateTime.Now,
                CreatePerson = userName
            };

            stock.Num = (stock.Num ?? 0) + Num;
            stock.Money = (stock.Money ?? 0) + UnitPrice*Num;
            stock.UnitPrice = stock.Money / (stock.Num == 0 ? 1 : stock.Num);
            stock.UpdateDate = DateTime.Now;
            stock.UpdatePerson = userName;

            var rowsAffect = context.Update<mms_warehouseStock>("mms_warehouseStock", stock)
                .AutoMap(x => x.WarehouseCode, x => x.MaterialCode)
                .Where(x => x.WarehouseCode).Where(x => x.MaterialCode)
                .Execute();

            if (rowsAffect == 0)
                rowsAffect = context.Insert<mms_warehouseStock>("mms_warehouseStock", stock).AutoMap().Execute();

            if (rowsAffect < 0)
                throw new Exception("更新仓库库存时发错误，请重试或联系管理员！");

            return rowsAffect;
        }

        public static int UpdateMaterialWarnning(IDbContext context,string projectCode, string MaterialCode,decimal Num)
        {
            var userName = MmsHelper.GetUserName();

            var builder = context.Select<mms_materialWarnning>("*")
                   .From("mms_materialWarnning")
                   .Where("ProjectCode=@0 and MaterialCode=@1")
                   .Parameters(projectCode, MaterialCode);

            mms_materialWarnning warnning = builder.QuerySingle() ?? new mms_materialWarnning()
            {
                ProjectCode = projectCode,
                MaterialCode = MaterialCode,
                LowerNum = 0,
                UpperNum = 0,
                Num = 0,
                CreatePerson = userName,
                CreateDate = DateTime.Now,
            };

            warnning.Num = (warnning.Num ?? 0) + Num;
            warnning.UpdateDate = DateTime.Now;
            warnning.UpdatePerson = userName;

            var rowsAffect = context.Update<mms_materialWarnning>("mms_materialWarnning", warnning)
                .AutoMap(x => x.ProjectCode, x => x.MaterialCode)
                .Where(x => x.ProjectCode).Where(x => x.MaterialCode)
                .Execute();

            if (rowsAffect == 0)
                rowsAffect = context.Insert<mms_materialWarnning>("mms_materialWarnning", warnning).AutoMap().Execute();

            if (rowsAffect < 0)
                throw new Exception("更新项目库存时发错误，请重试或联系管理员！");

            return rowsAffect;
        }

        public static int UpdateWarehouseStock(IDbContext context, string masterTableName, string billNo,bool isPlus = true)
        {
            var result = 0;
            var rows = context.Sql(string.Format(@"
select A.*,B.WarehouseCode,B.ProjectCode 
from {0}Detail A 
left join {0} B on B.BillNo = A.BillNo 
where A.BillNo='{1}'", masterTableName, billNo)).QueryMany<dynamic>();

            foreach (var item in rows)
            {
                decimal Num = isPlus ? item.Num : (0 - item.Num);
                result += UpdateWarehouseStock(context, item.WarehouseCode, item.MaterialCode,item.Unit,item.UnitPrice, Num);//更新仓库库存
                UpdateMaterialWarnning(context, item.ProjectCode, item.MaterialCode, Num);//更新项目库存

                var history = new mms_warehouseStockHistory()
                {
                    SrcBillType = masterTableName.Replace("mms_",""),
                    SrcBillNo = item.BillNo,
                    SrcRowId = item.RowId,
                    MaterialCode = item.MaterialCode,
                    WarehouseCode = item.WarehouseCode,
                    Num = Num,
                    UnitPrice = item.UnitPrice,
                    Money = Num * item.UnitPrice
                };
                mms_warehouseStockHistoryService.InsertWarehouseStockHistory(context, history);
            }
            return result;
        }
    }

    public class mms_warehouseStock : ModelBase
    {
        [PrimaryKey]
        public string WarehouseCode{ get; set; }
        [PrimaryKey]
        public string MaterialCode{ get; set; }
        public string Unit{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Money{ get; set; }
        public decimal? WarnStockDown{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}

 
