using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    public class sys_rolePermissionMapService : ServiceBase<sys_rolePermissionMap>
    {
       
    }

    public class sys_rolePermissionMap : ModelBase
    {

        [Identity]
        [PrimaryKey]
        public int ID{ get; set; }
        public string RoleCode{ get; set; }
        public string PermissionCode{ get; set; }
        public bool IsDefault { get; set; }
    }
}
