using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_materialWarnningService : ServiceBase<mms_materialWarnning>
    {
       
    }

    public class mms_materialWarnning : ModelBase
    {

        [PrimaryKey]
        public string ProjectCode{ get; set; }
        [PrimaryKey]
        public string MaterialCode{ get; set; }
        public decimal? LowerNum{ get; set; }
        public decimal? UpperNum{ get; set; }
        public decimal? Num{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
