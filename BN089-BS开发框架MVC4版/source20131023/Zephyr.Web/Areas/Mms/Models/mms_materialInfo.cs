using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using Zephyr.Utils;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_materialService : ServiceBase<mms_material>
    {
        public string GetNewMaterialCode(string ParentCode)
        {
            int index = 0;
            var MaxCode = db.Sql(@"select max(MaterialCode) 
from mms_material 
where MaterialType=@0", ParentCode).QuerySingle<string>()??string.Empty;
          
           
            if (MaxCode.Length < ParentCode.Length)
            {
                index = ZConvert.To<int>(MaxCode, 0);
            }
            else
            {
                index = ZConvert.To<int>(MaxCode.Substring(ParentCode.Length), 0);
            }
            return ParentCode + (index+1).ToString().PadLeft(2, '0');
        }
       
    }

    public class mms_material : ModelBase
    {

        [PrimaryKey]
        public string MaterialCode{ get; set; }
        public string MaterialName{ get; set; }
        public string MaterialType{ get; set; }
        public string Model{ get; set; }
        public string Unit{ get; set; }
        public string Material{ get; set; }
        public string LimitLevel{ get; set; }
        public string ManageLevel{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
