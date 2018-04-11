using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_rentInDetailService : ServiceBase<mms_rentInDetail>
    {
       
    }

    public class mms_rentInDetail : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        [PrimaryKey]
        public int RowId{ get; set; }
        public string MaterialCode{ get; set; }
        public string Unit{ get; set; }
        public string UseUnit{ get; set; }
        public string PriceUnit { get; set; }
        public decimal? Num { get; set; }
        public decimal? UnitPrice{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
