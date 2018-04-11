using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_customerContractService : ServiceBase<psi_customerContract>
    {
       
    }

    public class psi_customerContract : ModelBase
    {

        [PrimaryKey]
        public string ContractNo{ get; set; }
        public string CustomerId{ get; set; }
        public string ContractName{ get; set; }
        public string Catagory{ get; set; }
        public string ContractState{ get; set; }
        public string PayKind{ get; set; }
        public decimal? PrePay{ get; set; }
        public decimal? ContractAmount{ get; set; }
        public decimal? Payed{ get; set; }
        public decimal? NoPay{ get; set; }
        public DateTime? BeginDate{ get; set; }
        public int? Deadline{ get; set; }
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
