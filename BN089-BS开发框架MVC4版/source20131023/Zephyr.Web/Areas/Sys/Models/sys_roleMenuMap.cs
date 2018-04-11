using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_roleMenuMapService : ServiceBase<sys_roleMenuMap>
    {
       
    }
    
    public class sys_roleMenuMap : ModelBase
    {

        [Identity]
        [PrimaryKey]
        public int ID
        {
            get;
            set;
        }

        public string RoleCode
        {
            get;
            set;
        }

        public string MenuCode
        {
            get;
            set;
        }

    }
}
