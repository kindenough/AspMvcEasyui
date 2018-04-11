using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_organizeRoleMapService : ServiceBase<sys_organizeRoleMap>
    {
       
    }

    public class sys_organizeRoleMap : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID{get;set;}
        public string OrganizeCode{get;set;}
        public string RoleCode{get;set;}
    }
}
