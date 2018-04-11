using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Data;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_warehouseStockHistoryService : ServiceBase<mms_warehouseStockHistory>
    {
        public static int InsertWarehouseStockHistory(IDbContext context,mms_warehouseStockHistory history)
        {
            var userName = MmsHelper.GetUserName();
            history.CreateDate = DateTime.Now;
            history.CreatePerson = userName;
            history.UpdateDate = DateTime.Now;
            history.UpdatePerson = userName;

            var stock = context.Select<mms_warehouseStock>("Num,UnitPrice")
                .From("mms_warehouseStock")
                .Where("WarehouseCode=@0 and MaterialCode=@1")
                .Parameters(history.WarehouseCode, history.MaterialCode)
                .QuerySingle();

            if (stock != null)
            {
                history.StockNum = stock.Num;
                history.StockUnitPrice = stock.UnitPrice;
                history.StockMoney = stock.Money;
            }

           var result = context.Insert<mms_warehouseStockHistory>("mms_warehouseStockHistory", history).AutoMap(x => x.Id).Execute();
           return result;
        }
       
    }

    public class mms_warehouseStockHistory : ModelBase
    {

        [Identity]
        public int Id{ get; set; }
        public string SrcBillType{ get; set; }
        public string SrcBillNo{ get; set; }
        public int? SrcRowId{ get; set; }
        public string MaterialCode{ get; set; }
        public string WarehouseCode{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Money { get; set; }
        public decimal? StockNum{ get; set; }
        public decimal? StockUnitPrice{ get; set; }
        public decimal? StockMoney { get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
