using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_pickingGoodsService : ServiceBase<psi_pickingGoods>
    {
       
    }

    public class psi_pickingGoods : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        [PrimaryKey]
        public string RowId{ get; set; }
        public string GoodNo{ get; set; }
        public string Unit{ get; set; }
        public string Model{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Money{ get; set; }
        public string ReceiveBillNo{ get; set; }
        public string ReceiveRowId{ get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
