using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_dealService : ServiceBase<mms_deal>
    {
       
    }

    public class mms_deal : ModelBase
    {
        [PrimaryKey]   
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public string DoPerson { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string DealType { get; set; }
        public string DealKind { get; set; }
        public decimal? TotalMoney { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Remark { get; set; }
    }
}
