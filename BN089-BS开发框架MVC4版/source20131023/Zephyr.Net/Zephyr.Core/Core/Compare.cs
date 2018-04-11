/*************************************************************************
 * 文件名称 ：Compare.cs                          
 * 描述说明 ：定义查询条件中的比较符号
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public static class CpHelper
    {
        private static Dictionary<string, Func<WhereData, string>> _dictionary = new Dictionary<string, Func<WhereData, string>>();
        static CpHelper() {
            var methods = typeof(Cp).GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var item in methods)
            {
                var temp = item;
                _dictionary.Add(item.Name.ToLower(), data => (string)temp.Invoke(null, new object[] { data }));
            }
        }

        public static Func<WhereData, string> Parse(string str, Func<WhereData, string> defaultCp = null)
        {
            str = (str ?? string.Empty).ToLower();
            return _dictionary.ContainsKey(str) ? _dictionary[str] : (defaultCp??Cp.Equal);
        }
    }
 
    public partial class Cp
    {
        public static string Equal(WhereData data)          { return SQL(data, "{0} =  '{1}'"); }
        public static string NotEqual(WhereData data)       { return SQL(data, "{0} <> '{1}'"); }
        public static string Greater(WhereData data)        { return SQL(data, "{0} >  '{1}'"); }
        public static string GreaterEqual(WhereData data)   { return SQL(data, "{0} >= '{1}'"); }
        public static string DtGreaterEqual(WhereData data) { return SQL(data, "datediff(day,'{1}',{0}) > =0"); }
        public static string Less(WhereData data)           { return SQL(data, "{0} < '{1}'"); }
        public static string LessEqual(WhereData data)      { return SQL(data, "{0} <=  '{1}'"); }
        public static string DtLessEqual(WhereData data)    { return SQL(data, "datediff(day,'{1}',{0}) < =0"); }
        public static string In(WhereData data)             { return SQL(data, "{0} in ({1})"); }
        public static string Like(WhereData data)           { return SQL(data, "{0} like '%{1}%'"); }
        public static string StartWith(WhereData data)      { return SQL(data, "{0} like '{1}%'"); }
        public static string StartWithPY(WhereData data)    { return SQL(data, "{0} like '{1}%' or [dbo].[fun_getPY]({0}) like N'{1}%'"); }
        public static string EndWith(WhereData data)        { return SQL(data, "{0} like '%{1}'"); }
        public static string Between(WhereData data)        { return SQL(data, "{0} between '{1}'  '{0}'"); }
        public static string DateRange(WhereData data)      { return GetDateRangeSql(data, '到'); }
        public static string Map(WhereData data)            { return SQL(data, "{0} in (select {0} from {0} where {0}='{1}')"); }
        public static string Child(WhereData data)          { return SQL(data, "{0} in (select ID from [dbo].[GetChild]('{0}','{1}'))"); }
        public static string MapChild(WhereData data)       { return SQL(data, "{0} in (select {0} from {2} where {3} in (select ID from [dbo].[GetChild]('{4}','{1}')))"); }
 
        private static string SQL(WhereData cp, string stringFormat)
        {
            var list = cp.Extend.ToList();
            list.Insert(0, cp.Value);
            list.Insert(0, cp.Column);
            return string.Format(stringFormat, list.ToArray());
        }

        private static string GetDateRangeSql(WhereData cp, char _separator, bool _ignoreEmpty = true)
        {
            var sSql = string.Empty;
            var _from = "datediff(day,'{1}',{0}) >=0";
            var _to = "datediff(day,'{1}',{0})<=0";
            
            var values = ZConvert.ToString(cp.Value).Split(_separator);

            if (values.Length == 1)
                values = new string[] { values[0], values[0] };

            if (!string.IsNullOrWhiteSpace(values[0]) || !_ignoreEmpty)
                sSql = string.Format(_from,cp.Column, values[0]);

            if (!string.IsNullOrWhiteSpace(values[1]) || !_ignoreEmpty)
                sSql += (sSql.Length > 0 ? " and " : string.Empty) + string.Format(_to, cp.Column, values[1]);

            return sSql;
        }
    }
}
