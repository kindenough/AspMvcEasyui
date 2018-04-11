using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_purchaseService : ServiceBase<psi_purchase>
    {
       
    }

    public class psi_purchase : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public DateTime? DoDate{ get; set; }
        public string Supplier{ get; set; }
        public string ContractName{ get; set; }
        public DateTime? PurchaseDate{ get; set; }
        public string PurchasePerson{ get; set; }
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
