using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Zephyr.Models
{
    public class sys_menuService : ServiceBase<sys_menu>
    {
        protected override bool OnBeforEditDetail(EditEventArgs arg)
        {
            var MenuCode = arg.row["_id"].ToString();

            if (arg.type == OptType.Del)
            {
                db.Sql(@"
--删除角色字段Map
delete sys_roleMenuColumnMap
where id in 
(
select sys_roleMenuColumnMap.id
from sys_roleMenuColumnMap
left join sys_menu on sys_menu.menucode=sys_roleMenuColumnMap.menucode
where sys_menu.menucode=@0
)

--删除角色菜单Map
delete sys_roleMenuMap
where id in 
(
select sys_roleMenuMap.id
from sys_roleMenuMap
left join sys_menu on sys_menu.menucode=sys_roleMenuMap.menucode
where sys_menu.menucode=@0
)

--删除菜单按钮Map
delete sys_menuButtonMap
where sys_menuButtonMap.id in 
(
select id 
from sys_menuButtonMap
left join sys_menu on sys_menu.menucode=sys_menuButtonMap.menucode
where sys_menu.menucode=@0
) ", MenuCode).Execute();
            }

            return base.OnBeforEditDetail(arg);
        }

        //权限菜单
        public dynamic GetUserMenu(string UserCode) 
        { 
            var sql = String.Format(@"
--角色的菜单
select distinct B.*
from sys_roleMenuMap A
inner join sys_menu  B on B.MenuCode = A.MenuCode
where B.IsEnable='1'
  and RoleCode in (
  select RoleCode from sys_userRoleMap where UserCode = '{0}' --用户的角色
union all
  select RoleCode from sys_organizeRoleMap where OrganizeCode in  --机构的角色
  (
	select OrganizeCode from sys_userOrganizeMap where UserCode = '{0}'
  )  
)
order by B.MenuSeq,B.MenuCode", UserCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        public dynamic GetEnabledMenusAndButtons(string RoleCode)
        {
            var buttons = db.Sql("select * from sys_button order by ButtonSeq").QueryMany<sys_button>();

            var sql = "select A.MenuName,A.MenuCode,A.ParentCode,A.IconClass as iconCls,B.MenuName as ParentName";
            sql += String.Format(",(select 1 from sys_roleMenuMap tb_role where tb_role.RoleCode='{0}' and tb_role.MenuCode=A.MenuCode) as checked",RoleCode);
            foreach (var button in buttons)
                sql += String.Format(@",(
select case when max(tb1_{0}.ID) is null then -1 
            when max(tb2_{0}.ID) is null then 0 
            else 1 end 
from  sys_menuButtonMap AS tb1_{0}
left join sys_roleMenuButtonMap AS tb2_{0} ON tb2_{0}.MenuCode=tb1_{0}.MenuCode AND tb2_{0}.ButtonCode=tb1_{0}.ButtonCode AND tb2_{0}.RoleCode='{1}'
where tb1_{0}.MenuCode = A.MenuCode and  tb1_{0}.ButtonCode = '{0}'  
)as 'btn_{0}' ", button.ButtonCode, RoleCode);

            sql += @"
from sys_menu as A
left join sys_menu B on B.MenuCode = A.ParentCode
where A.IsEnable = 1
order by A.MenuSeq,A.MenuCode";

            var result = db.Sql(sql).QueryMany<dynamic>();


            var columns = db.Sql("select * from sys_roleMenuColumnMap where RoleCode = @0", RoleCode).QueryMany<sys_roleMenuColumnMap>();

            foreach (var item in result)
            {
                string MenuCode = item.MenuCode;
                item.AllowColumns = string.Join(",", columns.Where(x => x.MenuCode == MenuCode && x.IsReject == false).Select(x => x.FieldName));
                item.RejectColumns = string.Join(",", columns.Where(x => x.MenuCode == MenuCode && x.IsReject == true).Select(x => x.FieldName));
            }

            return new {menus = result,buttons=buttons };
        }

        public dynamic GetMenuButtons(string MenuCode)
        {
            var sql = String.Format(@"
select A.*,
       case when B.ID is null then 0 else 1 end as Selected
from sys_button A
left join sys_menuButtonMap B on B.MenuCode = '{0}' and B.ButtonCode = A.ButtonCode
order by ButtonSeq", MenuCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        public void SaveMenuButtons(string MenuCode, JToken ButtonList)
        {
            db.UseTransaction(true);
            Logger("设置菜单按钮", () =>
            {
                db.Delete("sys_menuButtonMap").Where("MenuCode", MenuCode).Execute();
                foreach (JToken item in ButtonList.Children())
                {
                    var ButtonCode = item["ButtonCode"].ToString();
                    db.Insert("sys_menuButtonMap").Column("MenuCode", MenuCode).Column("ButtonCode", ButtonCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }

        public string GetCurrentMenuCode()
        {
            var url = HttpContext.Current.Request.RawUrl;
            var result = db.Sql("select MenuCode from sys_menu where URL = @0", url).QuerySingle<string>();
            return result;
        }

        public List<sys_button> GetCurrentUserMenuButtons()
        {
            var MenuCode = GetCurrentMenuCode();
            var UserCode = FormsAuth.GetUserData().UserCode;
            var sql = @"
select A.*
from sys_button A
inner join sys_roleMenuButtonMap B on B.MenuCode = @0 and B.ButtonCode = A.ButtonCode
where RoleCode in (
select RoleCode
from sys_userRoleMap
where userCode = @1
union
select A.RoleCode
from sys_organizeRoleMap A
inner join sys_userOrganizeMap B on B.OrganizeCode = A.OrganizeCode
where B.UserCode = @1
)
order by ButtonSeq";

            var result = db.Sql(sql,MenuCode,UserCode).QueryMany<sys_button>();
            return result;
        }
    }
    
    public class sys_menu : ModelBase
    {

        [PrimaryKey]
        public string MenuCode
        {
            get;
            set;
        }

        public string ParentCode
        {
            get;
            set;
        }

        public string MenuName
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public string IconClass
        {
            get;
            set;
        }

        public string IconURL
        {
            get;
            set;
        }

        public string MenuSeq
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool? IsVisible
        {
            get;
            set;
        }

        public bool? IsEnable
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
