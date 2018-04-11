using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_dealDetailService : ServiceBase<mms_dealDetail>
    {
       
    }

    public class mms_dealDetail : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        [PrimaryKey]
        public int RowId{ get; set; }
        public string MaterialName{ get; set; }
        public string Model{ get; set; }
        public string Unit{ get; set; }
        public string ExpendCompany { get; set; }
        public DateTime? DealDate{ get; set; }
        public decimal? UnitPrice{ get; set; }
        public decimal? Num{ get; set; }
        public decimal? Money{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
