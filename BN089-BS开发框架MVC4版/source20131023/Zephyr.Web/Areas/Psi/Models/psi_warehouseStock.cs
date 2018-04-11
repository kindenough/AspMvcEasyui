using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_warehouseStockService : ServiceBase<psi_warehouseStock>
    {
       
    }

    public class psi_warehouseStock : ModelBase
    {

        [PrimaryKey]
        public string Id{ get; set; }
        [PrimaryKey]
        public string GoodNo{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Money{ get; set; }
        public decimal? WarnStock{ get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
