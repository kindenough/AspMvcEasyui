using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Data;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_rentOutService : ServiceBase<mms_rentOut>
    {
        protected override bool OnBeforeDelete(DeleteEventArgs arg)
        {
            var billNo = arg.data.GetValue("BillNo");
            arg.db.Delete("mms_rentOutDetail").Where("BillNo", billNo).Execute();
            arg.db.Delete("mms_rentOutRepaire").Where("BillNo", billNo).Execute();
            arg.db.Delete("mms_rentOutReparation").Where("BillNo", billNo).Execute();
            return base.OnBeforeDelete(arg);
        }

        //插入ProjectCode
        protected override bool OnBeforEditMaster(EditEventArgs arg)
        {
            arg.form["ProjectCode"] = MmsHelper.GetCurrentProject();
            return base.OnBeforEditMaster(arg);
        }

        //出场保存之后，更新进场单中的剩余数量
        protected override void OnAfterEditDetail(EditEventArgs arg)
        {
            var userName = MmsHelper.GetUserName();
            decimal num = arg.row.Value<decimal>("Num") - (arg.rowOld.Num ?? 0);
            string srcBillNo = arg.row.Value<string>("SrcBillNo");
            int srcRowId = arg.row.Value<int>("SrcRowId");
            arg.db.Sql(string.Format(@"
update mms_rentInDetail
set RemainNum = isnull(RemainNum,0) - {0} ,
UpdateDate = getdate(),
UpdatePerson = '{1}'
where BillNo='{2}' 
and RowId='{3}'", num, userName, srcBillNo, srcRowId)).Execute();
            
            base.OnAfterEditDetail(arg);
        }

        public static int CalcRentOutMoney(IDbContext db, string BillNo)
        {
            var result = 0;
            var rentout = db.Sql("select * from mms_rentOut where BillNo=@0", BillNo).QuerySingle<mms_rentOut>();
            var rentoutDetail = db.Sql("select * from mms_rentOutDetail where BillNo=@0", BillNo).QueryMany<mms_rentOutDetail>();
            
            foreach (var row in rentoutDetail)
            {
                var rentin = db.Sql(@"
select A.*,B.RentInDate 
from mms_rentInDetail A
left join mms_rentIn  B on B.BillNo = A.BillNo
where A.BillNo = @0
and A.RowId = @1 ", row.SrcBillNo,row.SrcRowId).QuerySingle<dynamic>();

                var rentoff = db.Sql(@"
select A.*,B.BeginDate,B.EndDate
from mms_rentOffDetail A
left join mms_rentOff  B on B.BillNo = A.BillNo
where A.SrcBillNo = @0
and A.SrcRowId = @1", row.SrcBillNo, row.SrcRowId).QueryMany<dynamic>();

                decimal diffMoney = 0;
                decimal rentinPrice = rentin.UnitPrice ?? 0;
                foreach (var item in rentoff)
                {
                    int rentoffDays = DateTime.Compare(item.EndDate, item.BeginDate);
                    decimal rentoffPrice = item.UnitPrice ?? 0;
                    decimal rentoffNum = item.Num ?? 0;
                    diffMoney += (rentinPrice - rentoffPrice) * rentoffNum * rentoffDays;
                }
               
                TimeSpan timeSpan = (rentout.RentOutDate ?? DateTime.Now) - (rentin.RentInDate ?? DateTime.Now);
                row.Money = row.Num * (timeSpan.Days + 1) * rentinPrice - diffMoney;

                result = db.Update("mms_rentoutDetail")
                    .Column("Money", row.Money)
                    .Where("BillNo", row.BillNo)
                    .Where("RowId",row.RowId).Execute();

                if (result < 0) return result;
            }

            result = db.Sql(@"
update mms_rentout
set TotalMoney = (select sum(Money) from mms_rentoutDetail where BillNo=@0)
where BillNo = @0",BillNo).Execute();

            return result;
        }
    }

    public class mms_rentOut : ModelBase
    {

        [PrimaryKey]
        public string BillNo{ get; set; }
        public DateTime? BillDate{ get; set; }
        public string DoPerson{ get; set; }
        public string ProjectCode{ get; set; }
        public DateTime? RentOutDate{ get; set; }
        public string ContractCode{ get; set; }
        public string SupplierCode{ get; set; }
        public decimal? TotalMoney { get; set; }
        public bool? IsTotal{ get; set; }
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
