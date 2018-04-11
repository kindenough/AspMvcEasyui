using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_userOrganizeMapService : ServiceBase<sys_userOrganizeMap>
    {
       
    }
    
    public class sys_userOrganizeMap : ModelBase
    {

        [Identity]
        [PrimaryKey]
        public int ID
        {
            get;
            set;
        }

        public string OrganizeCode
        {
            get;
            set;
        }

        public string UserCode
        {
            get;
            set;
        }

    }
}
