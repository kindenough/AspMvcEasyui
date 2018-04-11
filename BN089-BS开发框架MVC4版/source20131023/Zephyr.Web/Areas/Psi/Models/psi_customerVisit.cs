using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_customerVisitService : ServiceBase<psi_customerVisit>
    {
       
    }

    public class psi_customerVisit : ModelBase
    {

        [PrimaryKey]
        public string VisitId{ get; set; }
        public string CustomerId{ get; set; }
        public string VisitPerson{ get; set; }
        public DateTime? VisitDate{ get; set; }
        public string PickingBillNo{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
