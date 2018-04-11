/*************************************************************************
 * 文件名称 ：ParamUpdate.cs                          
 * 描述说明 ：更新参数构建
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
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Zephyr.Core
{
    public class ParamUpdate
    {
        protected ParamUpdateData data;

        public dynamic this[string index]
        {
            get { return data.Columns[index]; }
            set { data.Columns[index] = value; }
        }

        public ParamUpdate Update(string tableName)
        {
            data.Update = tableName;
            return this;
        }

        public ParamUpdate Column(string columnName, object value)
        {
            if (value != null && value.GetType() == typeof(JValue))
            {
                value = ((JValue)value).ToString();
            }
            data.Columns[columnName] = value;
            return this;
        }

        public ParamUpdate AndWhere(string column, object value, Func<WhereData,string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "and", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamUpdate OrWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "or", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamUpdate()
        {
            data = new ParamUpdateData();
        }

        public static ParamUpdate Instance()
        {
            return new ParamUpdate();
        }

        public dynamic GetDynamicValue()
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            foreach (var c in this.data.Columns)
                expando.Add(c.Key, c.Value);

            return (ExpandoObject)expando;
        }

        public ParamUpdateData GetData()
        {
            return data;
        }

    }
}
