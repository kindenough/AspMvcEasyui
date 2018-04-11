/*************************************************************************
 * 文件名称 ：ParamDelete.cs                          
 * 描述说明 ：删除参数构建
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;

namespace Zephyr.Core
{
    public class ParamDelete
    {
        protected ParamDeleteData data;

        public ParamDelete From(string sql)
        {
            data.From = sql;
            return this;
        }


        public ParamDelete AndWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {

            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "and", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamDelete OrWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "or", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }
  
        public ParamDelete()
        {
            data = new ParamDeleteData();
        }

        public static ParamDelete Instance()
        {
            return new ParamDelete();
        }

        public ParamDeleteData GetData()
        {
            return data;
        }
     }
}
