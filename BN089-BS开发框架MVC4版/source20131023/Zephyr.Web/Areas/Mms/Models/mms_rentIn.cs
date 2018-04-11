using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_rentInService : ServiceBase<mms_rentIn>
    {
        //删除前删除子表
        protected override bool OnBeforeDelete(DeleteEventArgs arg)
        {
            arg.db.Delete("mms_rentInDetail").Where("BillNo", arg.data.GetValue("BillNo")).Execute();
            return base.OnBeforeDelete(arg);
        }

        //保存ProjectCode
        protected override bool OnBeforEditMaster(EditEventArgs arg)
        {
            arg.form["ProjectCode"] = MmsHelper.GetCurrentProject();
            return base.OnBeforEditMaster(arg);
        }

        //给RemainNum赋初始值
        protected override bool OnBeforEditDetail(EditEventArgs arg)
        {
            if (arg.type == OptType.Add)
                arg.row["RemainNum"] = arg.row["Num"];
            else if (arg.type == OptType.Mod)
                arg.row["RemainNum"] = arg.row.Value<decimal>("Num") - (arg.rowOld.Num ?? 0) + (arg.rowOld.RemainNum ?? 0);

            return true;
        }

    }

    public class mms_rentIn : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public string ProjectCode{ get; set; }
        public DateTime? RentInDate{ get; set; }
        public string SupplierCode{ get; set; }
        public string ContractCode{ get; set; }
        public string BuildPartCode{ get; set; }
        public string Purpose{ get; set; }
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
