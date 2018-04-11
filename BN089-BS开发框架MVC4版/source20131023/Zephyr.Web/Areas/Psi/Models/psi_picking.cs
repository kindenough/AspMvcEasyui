using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_pickingService : ServiceBase<psi_picking>
    {
       
    }

    public class psi_picking : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public DateTime? DoDate{ get; set; }
        public string CustomNo{ get; set; }
        public string Contract{ get; set; }
        public string Warehouse{ get; set; }
        public DateTime? PickDate{ get; set; }
        public string PickPerson{ get; set; }
        public string Purpose{ get; set; }
        public decimal? TotalMoney{ get; set; }
        public string Remark{ get; set; }
        public string AuditPerson{ get; set; }
        public DateTime? AuditDate{ get; set; }
        public string AuditState{ get; set; }
        public string AuditReason{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
