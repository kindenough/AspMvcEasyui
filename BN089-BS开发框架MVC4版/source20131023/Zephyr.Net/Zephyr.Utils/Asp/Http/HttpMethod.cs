using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.IO;
using System.IO.Compression;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

namespace Zephyr.Utils
{
    /// <summary>
    /// WEB请求上下文信息工具类
    /// </summary>
    public partial class ZHttp
    {
        #region 判断当前页面是否接收到了Post请求
        /// <summary>
        /// 判断当前页面是否接收到了Post请求,如果当前请求不存在,返回false
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            }
        }
        #endregion

        #region 判断当前页面是否接收到了Get请求
        /// <summary>
        /// 判断当前页面是否接收到了Get请求,如果当前请求不存在,返回 false
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            }
        }
        #endregion

        #region 获取客户端使用的HTTP数据传输方法
        /// <summary>
        /// 获取客户端使用的HTTP数据传输方法 
        /// </summary>
        public static string Method
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod;
            }
        }
        #endregion
    }
}

