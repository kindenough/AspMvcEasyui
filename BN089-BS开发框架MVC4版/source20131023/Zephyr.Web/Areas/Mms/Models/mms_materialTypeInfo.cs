using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_materialTypeService : ServiceBase<mms_materialType>
    {
       
    }

    public class mms_materialType : ModelBase
    {

        [PrimaryKey]
        public string MaterialType{ get; set; }
        public string MaterialTypeName{ get; set; }
        public string ParentCode{ get; set; }
        public string Type{ get; set; }
        public string Unit{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
