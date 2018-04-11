using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_codeService : ServiceBase<sys_code>
    {
       
    }

    public class sys_code : ModelBase
    {
        [PrimaryKey]   
        public string Code { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string ParentCode { get; set; }
        public string Seq { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsDefault { get; set; }
        public string Description { get; set; }
        public string CodeTypeName { get; set; }
        public string CodeType { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
