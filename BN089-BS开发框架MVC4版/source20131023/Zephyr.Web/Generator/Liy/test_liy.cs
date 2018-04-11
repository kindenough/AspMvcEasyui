using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class test_liyService : ServiceBase<test_liy>
    {
       
    }

    public class test_liy : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? OutDateTime { get; set; }
        public bool? IsValid { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
