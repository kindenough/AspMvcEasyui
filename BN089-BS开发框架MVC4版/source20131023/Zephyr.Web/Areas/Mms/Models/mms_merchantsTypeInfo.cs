using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_merchantsTypeService : ServiceBase<mms_merchantsType>
    {
       
    }

    public class mms_merchantsType : ModelBase
    {

        [PrimaryKey]
        public string MerchantsTypeCode{ get; set; }
        public string MerchantsTypeName{ get; set; }
        public string MerchantsProperty{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
