using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Zephyr.Models
{
    public class sys_roleService : ServiceBase<sys_role>
    {
        public void EditRoleMenu(string RoleCode,JToken MenuList){
            db.UseTransaction(true);
            Logger("修改角色菜单权限", () =>
            {
                db.Delete("sys_roleMenuMap").Where("RoleCode", RoleCode).Execute();
                foreach (JToken item in MenuList.Children())
                {
                    var MenuCode = item["MenuCode"].ToString();
                    db.Insert("sys_roleMenuMap").Column("RoleCode", RoleCode).Column("MenuCode", MenuCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }

        public void EditRoleMenuButton(string RoleCode, JToken MenuButtonList)
        {
            db.UseTransaction(true);
            Logger("修改角色菜单按钮权限", () =>
            {
                db.Delete("sys_roleMenuButtonMap").Where("RoleCode", RoleCode).Execute();
                foreach (JToken item in MenuButtonList.Children())
                {
                    var MenuCode = item["MenuCode"].ToString();
                    var ButtonCode = item["ButtonCode"].ToString();
                    db.Insert("sys_roleMenuButtonMap").Column("RoleCode", RoleCode).Column("MenuCode", MenuCode).Column("ButtonCode",ButtonCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }

        public void EditRolePermission(string RoleCode, JToken PermissionList)
        {
            db.UseTransaction(true);
            Logger("修改角色数据权限", () =>
            {
                db.Delete("sys_rolePermissionMap").Where("RoleCode", RoleCode).Execute();
                foreach (JToken item in PermissionList.Children())
                {
                    var PermissionCode = item["PermissionCode"].ToString();
                    var IsDefault = item["IsDefault"].ToString();
                    db.Insert("sys_rolePermissionMap").Column("RoleCode", RoleCode).Column("PermissionCode", PermissionCode).Column("IsDefault", IsDefault).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }

        public void EditRoleMenuColumns(string RoleCode, JToken MenuColumnList)
        {
            db.UseTransaction(true);
            Logger("修改角色数据列权限", () =>
            {
                db.Delete("sys_roleMenuColumnMap").Where("RoleCode", RoleCode).Execute();
                foreach (JToken item in MenuColumnList.Children())
                {
                    var MenuCode = item["MenuCode"].ToString();
                    var AllowColumns = item["AllowColumns"].ToString().Split(',');
                    var RejectColumns = item["RejectColumns"].ToString().Split(',');
                    var list = new List<sys_roleMenuColumnMap>();
                    foreach (var field in AllowColumns)
                    {
                        if (string.IsNullOrEmpty(field)) continue;
                        var model = new sys_roleMenuColumnMap()
                        {
                            RoleCode = RoleCode,
                            MenuCode = MenuCode,
                            IsReject = false,
                            FieldName = field
                        };
                        db.Insert<sys_roleMenuColumnMap>("sys_roleMenuColumnMap", model).AutoMap(x => x.ID).Execute();
                    }

                    foreach (var field in RejectColumns)
                    {
                        if (string.IsNullOrEmpty(field)) continue;
                        var model = new sys_roleMenuColumnMap()
                        {
                            RoleCode = RoleCode,
                            MenuCode = MenuCode,
                            IsReject = true,
                            FieldName = field
                        };
                        db.Insert<sys_roleMenuColumnMap>("sys_roleMenuColumnMap", model).AutoMap(x => x.ID).Execute();
                    }
                }
                db.Commit();
            }, e => db.Rollback());
        }

        public void SaveRoleMembers(string RoleCode, JToken MemberList)
        {
            db.UseTransaction(true);
            Logger("设置角色成员", () =>
            {
                db.Delete("sys_userRoleMap").Where("RoleCode", RoleCode).Execute();
                db.Delete("sys_organizeRoleMap").Where("RoleCode", RoleCode).Execute();
                foreach (JToken item in MemberList.Children())
                {
                    var MemberCode = item["MemberCode"].ToString();
                    var MemberType = item["MemberType"].ToString();
                    if (MemberType.ToLower()=="user")
                        db.Insert("sys_userRoleMap").Column("RoleCode", RoleCode).Column("UserCode", MemberCode).Execute();
                    else
                        db.Insert("sys_organizeRoleMap").Column("RoleCode", RoleCode).Column("OrganizeCode", MemberCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }

//        public List<string> GetCurrentUserRoles()
//        {
//            var UserCode = FormsAuth.GetUserData().UserCode;
//            var sql = @"
//select RoleCode
//from sys_userRoleMap
//where userCode = @0
//
//union
//
//select A.RoleCode
//from sys_organizeRoleMap A
//inner join sys_userOrganizeMap B on B.OrganizeCode = A.OrganizeCode
//where B.UserCode = @0"

//            return db.Sql(sql,UserCode).QueryMany<string>();
//        }
    }
    
    public class sys_role : ModelBase
    {
        [PrimaryKey]
        public string RoleCode
        {
            get;
            set;
        }

        public string RoleSeq
        {
            get;
            set;
        }

        public string RoleName
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
