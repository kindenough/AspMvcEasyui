using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_lossReportService : ServiceBase<mms_lossReport>
    {
       
    }

    public class mms_lossReport : ModelBase
    {
        [PrimaryKey]   
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public string DoPerson { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? LossReportDate { get; set; }
        public string WarehouseCode { get; set; }
        public string MaterialType { get; set; }
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
