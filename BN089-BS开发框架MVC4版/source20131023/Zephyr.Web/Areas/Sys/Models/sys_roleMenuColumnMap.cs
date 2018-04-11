using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Zephyr.Core;

namespace Zephyr.Models
{
    public class sys_roleMenuColumnMapService : ServiceBase<sys_roleMenuColumnMap>
    {
        public List<sys_roleMenuColumnMap> GetCurrentUserMenuColumns()
        {
            var MenuCode =new sys_menuService().GetCurrentMenuCode();
            var UserCode = FormsAuth.GetUserData().UserCode;
            var sql = @"
select *
from sys_roleMenuColumnMap 
where MenuCode = @0
and RoleCode in (
select RoleCode
from sys_userRoleMap
where userCode = @1
union
select A.RoleCode
from sys_organizeRoleMap A
inner join sys_userOrganizeMap B on B.OrganizeCode = A.OrganizeCode
where B.UserCode = @1
)";

            var result = db.Sql(sql, MenuCode, UserCode).QueryMany<sys_roleMenuColumnMap>();
            return result;
        }
    }

    public class sys_roleMenuColumnMap : ModelBase
    {
        [PrimaryKey]
        public int ID{ get; set; }
        public string RoleCode{ get; set; }
        public string MenuCode{ get; set; }
        public bool? IsReject{ get; set; }
        public string FieldName{ get; set; }
    }
}
