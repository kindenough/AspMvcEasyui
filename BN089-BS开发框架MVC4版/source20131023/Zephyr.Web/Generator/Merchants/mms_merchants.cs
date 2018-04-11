using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_merchantsService : ServiceBase<mms_merchants>
    {
       
    }

    public class mms_merchants : ModelBase
    {
        [PrimaryKey]   
        public string MerchantsCode { get; set; }
        public string MerchantsName { get; set; }
        public string MerchantsTypeCode { get; set; }
        public string MerchantsTypeName { get; set; }
        public string ChargePerson { get; set; }
        public string ChargeTel { get; set; }
        public string RegisterFund { get; set; }
        public DateTime? BuildDate { get; set; }
        public string BusinessScope { get; set; }
        public string BusinessType { get; set; }
        public string QualificationLevel { get; set; }
        public string Bank { get; set; }
        public string BankNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactTel { get; set; }
        public string ContactPosition { get; set; }
        public string ContactAddr { get; set; }
        public string ContactPostCode { get; set; }
        public string ContactFax { get; set; }
        public string Website { get; set; }
        public string EMail { get; set; }
        public string MajorSupplier { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Remark { get; set; }
    }
}
