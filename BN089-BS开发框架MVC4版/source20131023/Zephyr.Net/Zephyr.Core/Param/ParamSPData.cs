/*************************************************************************
 * 文件名称 ：ParamSPData.cs                          
 * 描述说明 ：SP参数数据
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.Collections.Generic;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class ParamSPData
    {
        public string Name { get; set; }
        public Dictionary<string,object> Parameter { get; set; }
        public Dictionary<string, DataTypes> ParameterOut { get; set; }

        public ParamSPData()
        {
            Name = string.Empty;
            Parameter = new Dictionary<string, object>();
            ParameterOut = new Dictionary<string, DataTypes>();
        }
    }
}
