/*************************************************************************
 * 文件名称 ：ParamInsert.cs                          
 * 描述说明 ：插入参数构建
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.Collections.Generic;
using System.Dynamic;

namespace Zephyr.Core
{
    public class ParamInsert
    {
        protected ParamInsertData data;

        public dynamic this[string index]
        {
            get { return data.Columns[index]; }
            set { data.Columns[index] = value; }
        }

        public ParamInsert Insert(string tableName)
        {
            data.From = tableName;
            return this;
        }

        public ParamInsert Column(string columnName, object value)
        {
            data.Columns[columnName] = value;
            return this;
        }

        public ParamInsert()
        {
            data = new ParamInsertData();
        }

        public static ParamInsert Instance()
        {
            return new ParamInsert();
        }

        public dynamic GetDynamicValue()
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            foreach (var c in this.data.Columns)
                expando.Add(c.Key, c.Value);

            return (ExpandoObject)expando;
        }

        public ParamInsertData GetData()
        {
            return data;
        }
    }
}
