/*************************************************************************
 * 文件名称 ：NewKey.cs                          
 * 描述说明 ：采番类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Zephyr.Data;
using Zephyr.Utils;

namespace Zephyr.Core
{
    //采番
    public class NewKey
	{
        public static string datetime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
        }

        public static string guid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
 
        //最大值加一
        public static string maxplus(IDbContext db, string table, string field, ParamQuery pQuery)
        {
            //var where = pQuery.GetData().WhereSql;
            var sqlWhere = " where 1 = 1 ";
            if (pQuery != null)
                sqlWhere += " and " +  pQuery.GetData().WhereSql;
            var dbkey = db.Sql(String.Format("select isnull(max({0}),0) from {1} {2}", field, table, sqlWhere)).QuerySingle<string>();
            var cachedKeys = getCacheKey(table, field);
            var currentKey = maxOfAllKey(cachedKeys, ZConvert.ToString(dbkey));
            var key = ZConvert.ToString(currentKey + 1);
            SetCacheKey(table, field, key);
            return key;
        }

        //日期时间加上N位数字加一
        public static string dateplus(IDbContext db, string table, string field,string datestringFormat,int numberLength)
        {
            var dbkey = db.Sql(String.Format("select isnull(max({0}),0) from {1}", field, table)).QuerySingle<string>();
            var mykey = DateTime.Now.ToString(datestringFormat) + string.Empty.PadLeft(numberLength, '0');
            var cachedKeys = getCacheKey(table, field);
            var currentKey = maxOfAllKey(cachedKeys, ZConvert.ToString(dbkey), mykey);
            var key = ZConvert.ToString(currentKey + 1);
            SetCacheKey(table, field, key);
            return key;
        }
 
        private static string getCacheKey(string table, string field)
        {
            var tableKeys = getTableKeys(table);
            return getFieldKeys(tableKeys, field);
        }

        private static Dictionary<string, string> getTableKeys(string table)
        {
            var tableKeys = ZCache.GetCache(String.Format("currentkey_{0}", table)) as Dictionary<string, string>;
            if (null == tableKeys)
                tableKeys = new Dictionary<string, string>();

            return tableKeys;
        }

        private static string getFieldKeys(Dictionary<string, string> tableKeys, string field)
        {
            return tableKeys.ContainsKey(field) ? tableKeys[field] : "0";;
        }

        private static void SetCacheKey(string table, string field, string key)
        {
            var tableKeys = getTableKeys(table);
            var fieldKeys = getFieldKeys(tableKeys, field);
            tableKeys[field] = ZConvert.ToString(maxOfAllKey(fieldKeys,key));
            ZCache.SetCache(String.Format("currentkey_{0}", table), tableKeys);
        }

        private static Int64 maxOfAllKey(string cachedKeys, params string[] otherKey)
        {
            var keys = new List<string> {cachedKeys};
            keys.AddRange(otherKey);
            var max = keys.Max<object>(x => Zephyr.Utils.ZConvert.To<Int64>(x));
            return max;
        }
	}
}
