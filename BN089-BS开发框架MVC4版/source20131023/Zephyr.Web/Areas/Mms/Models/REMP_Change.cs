using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class REMP_ChangeService : ServiceBase<REMP_Change>
    {
       
    }

    public class REMP_Change : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int CHANID { get; set; }
        public int? EMPSID { get; set; }
        public DateTime? changedate { get; set; }
        public int? changetype { get; set; }
        public int? createtime { get; set; }
    }
}
