using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_rentOutDetailService : ServiceBase<mms_rentOutDetail>
    {
       
    }

    public class mms_rentOutDetail : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        [PrimaryKey]
        public int RowId{ get; set; }
        public string MaterialCode{ get; set; }
        public string Unit{ get; set; }
        public decimal? Num { get; set; }
        public decimal? Money { get; set; }
        public string SrcBillType { get; set; }
        public string SrcBillNo{ get; set; }
        public int? SrcRowId{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
