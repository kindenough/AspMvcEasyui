using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_claimService : ServiceBase<psi_claim>
    {
       
    }

    public class psi_claim : ModelBase
    {

        [PrimaryKey]
        public string id{ get; set; }
        public string VisitId{ get; set; }
        public string customerId{ get; set; }
        public decimal? ClaimMoney{ get; set; }
        public string ClaimKind{ get; set; }
        public string AuditPerson{ get; set; }
        public DateTime? AuditDate{ get; set; }
        public string AuditState{ get; set; }
        public string AuditReason{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
