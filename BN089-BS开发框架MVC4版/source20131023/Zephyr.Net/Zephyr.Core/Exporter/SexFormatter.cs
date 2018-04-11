/*************************************************************************
 * 文件名称 ：SexFormatter.cs                          
 * 描述说明 ：格式化示例
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;

namespace Zephyr.Core
{
    public class SexFormatter:IFormatter
    {
        public object Format(object value)
        {
            switch(Convert.ToString(value))
            {
                case "0":
                    return "纯爷们";
                case "1":
                    return "女汉子";
                default:
                    return "春哥";
            }
        }
    }
}
