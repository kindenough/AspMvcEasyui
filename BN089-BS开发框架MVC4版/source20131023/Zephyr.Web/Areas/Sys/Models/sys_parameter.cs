using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_parameterService : ServiceBase<sys_parameter>
    {
       
    }
    
    public class sys_parameter : ModelBase
    {

        [PrimaryKey]
        public string ParamCode
        {
            get;
            set;
        }

        public string ParamValue
        {
            get;
            set;
        }

        public bool? IsUserEditable
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string CreatePerson
        {
            get;
            set;
        }

        public DateTime? CreateDate
        {
            get;
            set;
        }

        public string UpdatePerson
        {
            get;
            set;
        }

        public DateTime? UpdateDate
        {
            get;
            set;
        }

    }
}
