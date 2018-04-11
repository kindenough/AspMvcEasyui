/*************************************************************************
 * 文件名称 ：ParamUtils.cs                          
 * 描述说明 ：参数工具类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Concurrent;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public class ParamUtils
    {
        private static readonly ConcurrentDictionary<Enum, string> _cachedCpEnum = new ConcurrentDictionary<Enum, string>();

        public static string GetEnumDescription(Enum enumSubitem)
        {
            var description = _cachedCpEnum.GetOrAdd(enumSubitem, _GetEnumDesc);
            return description;
        }

        private static string _GetEnumDesc(Enum enumSubitem)
        {
            var fieldinfo = enumSubitem.GetType().GetField(enumSubitem.ToString());
            var objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
                return enumSubitem.ToString();

            var da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        public static string GetWhereSql(List<ParamWhere> Where)
        {
            var sql = string.Empty;
            Where.ForEach(x =>
            {
                //var fieldinfo = x.Compare.GetType().GetField(x.Compare.ToString());
                //var objs = fieldinfo.GetCustomAttributes(typeof(ICompareAttribute), false);
                //if (objs.Length == 0)
                //    throw new ZException("compare item missing attribute with ICompareAttribute interface");
                //var Compare = (ICompareAttribute)objs[0];
                //sql += (string.IsNullOrEmpty(sql)?string.Empty:x.AndOr) + " " + Compare.GetCompareSql(x.Column,x.Value,x.Extend);

                sql += (string.IsNullOrEmpty(sql) ? string.Empty : x.Data.AndOr) + " " + x.Compare(x.Data);
            });

            return sql;
        }
    }
}
