using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_refundService : ServiceBase<mms_refund>
    {
        protected override bool OnBeforeDelete(DeleteEventArgs arg)
        {
            arg.db.Delete("mms_refundDetail").Where("BillNo", arg.data.GetValue("BillNo")).Execute();
            return base.OnBeforeDelete(arg);
        }

        //≤Â»ÎProjectCode
        protected override bool OnBeforEditMaster(EditEventArgs arg)
        {
            arg.form["ProjectCode"] = MmsHelper.GetCurrentProject();
            return base.OnBeforEditMaster(arg);
        }
    }

    public class mms_refund : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public string ProjectCode{ get; set; }
        public DateTime? RefundDate{ get; set; }
        public string WarehouseCode{ get; set; }
        public string MaterialType{ get; set; }
        public string RefundUnit{ get; set; }
        public string RefundPerson{ get; set; }
        public decimal? TotalMoney{ get; set; }
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
