using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_testService : ServiceBase<mms_test>
    {
       
    }

    public class mms_test : ModelBase
    {

        public string ID{ get; set; }
        public string Year{ get; set; }
        public string ProjectName{ get; set; }
        public string DeclaringUnits{ get; set; }
        public string ProjectType{ get; set; }
        public DateTime? StartDate{ get; set; }
        public DateTime? EndDate{ get; set; }
        public decimal? TotalMoney{ get; set; }
    }
}
