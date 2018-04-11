using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_productService : ServiceBase<mms_product>
    {
       
    }

    public class mms_product : ModelBase
    {

        public string ID{ get; set; }
        public string ProductName{ get; set; }
        public string Color{ get; set; }
        public decimal? Price{ get; set; }
        public string Unit{ get; set; }
        public decimal? Money{ get; set; }
        public int? Qty{ get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
