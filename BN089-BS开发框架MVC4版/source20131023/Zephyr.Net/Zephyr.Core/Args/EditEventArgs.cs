/*************************************************************************
 * 文件名称 ：EditEventArgs.cs                          
 * 描述说明 ：编辑事件参数
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using Newtonsoft.Json.Linq;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class EditEventArgs
    {
        public IDbContext db { get; set; }
        public JToken form { get; set; }
        public dynamic formOld { get; set; }
        public JToken row { get; set; }
        public dynamic rowOld { get; set; }
        public JToken list { get; set; }
        public RequestWrapper wrapper { get; set; }
        public OptType type { get; set; }
        public dynamic executeValue { get; set; }

        public EditEventArgs()
        {
            type = OptType.None;
        }
    }
}
