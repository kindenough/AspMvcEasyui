using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_logService : ServiceBase<sys_log>
    {
       
    }
    
    public class sys_log : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string Position { get; set; }

        public string Target { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public DateTime? Date { get; set; }

    }
}
