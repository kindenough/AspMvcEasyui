using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_checkDetailService : ServiceBase<mms_checkDetail>
    {
       
    }

    public class mms_checkDetail : ModelBase
    {
        [PrimaryKey]
        public string BillNo { get; set; }
        [PrimaryKey]
        public int RowId{ get; set; }
        public string MaterialCode{ get; set; }
        public string Unit{ get; set; }
        public decimal? BookNum{ get; set; }
        public decimal? BookUnitPrice{ get; set; }
        public decimal? BookMoney{ get; set; }
        public decimal? ActualNum{ get; set; }
        public decimal? ActualUnitPrice{ get; set; }
        public decimal? ActualMoney{ get; set; }
        public decimal? OperateNum{ get; set; }
        public decimal? OperateMoney{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
