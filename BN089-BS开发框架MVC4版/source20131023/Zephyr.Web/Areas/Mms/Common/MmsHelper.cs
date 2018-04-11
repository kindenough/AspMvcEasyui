using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Zephyr.Core;
using Zephyr.Utils;

namespace Zephyr.Web.Areas.Mms.Common
{
    public class MmsHelper
    {
        public static string GetCookies(string name) 
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(name);
            return cookie == null ? null : cookie.Value;
        }

        public static string GetUserName()
        {
            return FormsAuth.GetUserData().UserName;
        }

        public static string GetCurrentProject()
        {
            return GetCookies("CurrentProject");
        }

        public static void ThrowHttpExceptionWhen(bool condition,string message,params object[] param)
        {
            if (condition)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent(string.Format(message,param)) });
        }

        public static dynamic GetEditUrls(string controller,object extend=null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["getdetail"] =  string.Format("/api/mms/{0}/getdetail/",controller);
            expando["getmaster"] =  string.Format("/api/mms/{0}/geteditmaster/",controller);
            expando["edit"] =  string.Format("/api/mms/{0}/edit/",controller);
            expando["audit"] =  string.Format("/api/mms/{0}/audit/",controller);
            expando["getrowid"] =  string.Format("/api/mms/{0}/getnewrowid/",controller);
            expando["report"] = controller;
 
            if (extend!=null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetEditResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["rejected"] = "已撤消修改！";
            expando["editSuccess"] = "保存成功！";
            expando["auditPassed"] ="单据已通过审核！";
            expando["auditReject"] = "单据已取消审核！";
 
            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }

        public static dynamic GetIndexUrls(string controller,object extend=null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["query"] = string.Format("/api/mms/{0}",controller);
            expando["remove"] = string.Format("/api/mms/{0}/", controller);
            expando["billno"] = string.Format("/api/mms/{0}/getnewbillno", controller);
            expando["audit"] = string.Format("/api/mms/{0}/audit/", controller);
            expando["edit"] = string.Format("/mms/{0}/edit/", controller);
            if (extend!=null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetIndexResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["detailTitle"] = billName+ "明细";
            expando["noneSelect"] = "请先选择一条" + billName + "！";
            expando["deleteConfirm"] = "确定要删除选中的" + billName + "吗？";
            expando["deleteSuccess"] = "删除成功！";
            expando["auditSuccess"] = "单据已审核！";
 
            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }
    }
}