/*************************************************************************
 * 文件名称 ：ParamQuery.cs                          
 * 描述说明 ：查询参数构建
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Linq;

namespace Zephyr.Core
{
    public class ParamQuery
    {
        protected ParamQueryData data;

        public ParamQuery Select(string sql)
        {
            data.Select = sql;
            return this;
        }

        public ParamQuery From(string sql)
        {
            data.From = sql;
            return this;
        }
 
        //public ParamQuery AndWhere(string column, object value, Cp cp = Cp.equal, params object[] extend)
        //{
        //    data.Where.Add(new ParamWhere() { AndOr = "and", Column = column, Value = value, Compare = cp, Extend = extend });
        //    return this;
        //}

        public ParamQuery AndWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "and", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        //public ParamQuery OrWhere(string column, object value, Cp cp = Cp.equal, params object[] extend)
        //{
        //    data.Where.Add(new ParamWhere() { AndOr = "or", Column = column, Value = value, Compare = cp, Extend = extend });
        //    return this;
        //}

        public ParamQuery OrWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "or", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamQuery ClearWhere()
        {
            data.Where.Clear();
            return this;
        }
 
        public ParamQuery GroupBy(string sql)
        {
            data.GroupBy = sql;
            return this;
        }

        public ParamQuery OrderBy(string sql)
        {
            var sortOrder = sql.Trim().Split(' ');
            if (!string.IsNullOrWhiteSpace(sql))
            {
                string mainTable = null;
                data.Select.Trim().Split(',').ToList().ForEach(x => {
                    if (x.Trim().EndsWith("." + sortOrder[0])) sortOrder[0] = x;
                    if (x.Trim().EndsWith(".*")) mainTable = x.Split('.')[0];
                });

                if (mainTable !=null && mainTable.ToLower().StartsWith("distinct"))
                    mainTable = mainTable.Substring(8);

                if (sortOrder[0].IndexOf(".") == -1 && !string.IsNullOrEmpty(mainTable))
                    sortOrder[0] = mainTable + "." + sortOrder[0];
            }

            data.OrderBy = string.Join(" ", sortOrder);
            return this;
        }

        public ParamQuery Having(string sql)
        {
            data.Having = sql;
            return this;
        }

        public ParamQuery Paging(int currentPage, int itemsPerPage)
        {
            data.PagingCurrentPage = currentPage;
            data.PagingItemsPerPage = itemsPerPage;
            return this;
        }

        public ParamQuery()
        {
            data = new ParamQueryData();
        }

        public static ParamQuery Instance()
        {
            return new ParamQuery();
        }

        public ParamQueryData GetData()
        {
            return data;
        }

     }
}
