using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_customerService : ServiceBase<psi_customer>
    {
       
    }

    public class psi_customer : ModelBase
    {

        [PrimaryKey]
        public string Id{ get; set; }
        public string Name{ get; set; }
        public string ChargePerson{ get; set; }
        public string Tel{ get; set; }
        public string Addr{ get; set; }
        public string Catagory{ get; set; }
        public string MainService{ get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
