using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_checkService : ServiceBase<mms_check>
    {
        protected override bool OnBeforeDelete(DeleteEventArgs arg)
        {
            arg.db.Delete("mms_checkDetail").Where("BillNo", arg.data.GetValue("BillNo")).Execute();
            return base.OnBeforeDelete(arg);
        }
        //≤Â»ÎProjectCode
        protected override bool OnBeforEditMaster(EditEventArgs arg)
        {
            arg.form["ProjectCode"] = MmsHelper.GetCurrentProject();
            return base.OnBeforEditMaster(arg);
        }

    }

    public class mms_check : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public string ProjectCode{ get; set; }
        public string WarehouseCode{ get; set; }
        public string Years{ get; set; }
        public string Months{ get; set; }
        public string MaterialType{ get; set; }
        public decimal? BookMoney{ get; set; }
        public decimal? OperateMoney{ get; set; }
        public string ApproveState{ get; set; }
        public string ApprovePerson{ get; set; }
        public DateTime? ApproveDate{ get; set; }
        public string ApproveRemark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
