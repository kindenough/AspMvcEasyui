using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class REMP_DepartmentService : ServiceBase<REMP_Department>
    {
       
    }

    public class REMP_Department : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int DEPTID { get; set; }
        public string deptname { get; set; }
        public int? parentid { get; set; }
        public string remark { get; set; }
    }
}
