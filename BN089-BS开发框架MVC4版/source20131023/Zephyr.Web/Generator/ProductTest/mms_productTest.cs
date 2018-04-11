using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class mms_productTestService : ServiceBase<mms_productTest>
    {
       
    }

    public class mms_productTest : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductColor { get; set; }
        public string ProductType { get; set; }
        public DateTime? ProductDate { get; set; }
        public int? Qty { get; set; }
        public decimal? Money { get; set; }
    }
}
