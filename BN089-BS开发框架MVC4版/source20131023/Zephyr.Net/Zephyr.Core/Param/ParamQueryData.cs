/*************************************************************************
 * 文件名称 ：ParamQueryData.cs                          
 * 描述说明 ：查询参数数据
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
using Zephyr.Utils;

namespace Zephyr.Core
{
    public class ParamQueryData
    {
        public int PagingCurrentPage { get; set; }
        public int PagingItemsPerPage { get; set; }
        
        public string Having { get; set; }
        public string GroupBy { get; set; }
        public string OrderBy { get; set; }
        public string From { get; set; }
        public string Select { get; set; }
        public List<ParamWhere> Where { get; set; }
        public string WhereSql {get { return ParamUtils.GetWhereSql(Where); }}

        public ParamQueryData()
        {
            Having = "";
            GroupBy = "";
            OrderBy = "";
            From = "";
            Select = "";
            Where = new List<ParamWhere>();
            PagingCurrentPage = 1;
            PagingItemsPerPage = 0;
        }
    }
}
