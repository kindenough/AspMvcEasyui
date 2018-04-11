using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_repairDetailService : ServiceBase<mms_repairDetail>
    {
       
    }

    public class mms_repairDetail : ModelBase
    {
        [PrimaryKey]
        public string BillNo { get; set; }
        [PrimaryKey]
        public int RowId { get; set; }
        public string RepairNo{ get; set; }
        public string RepairName{ get; set; }
        public string Unit{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? Money{ get; set; }
        public string RepairReason{ get; set; }
        public decimal? LaborCost{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
