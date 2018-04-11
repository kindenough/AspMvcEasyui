using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.WorkFlow
{
    public enum EventType
    {
        //事件类型

        //进入
        Enter,

        //离开
        Leave,

        //分派
        Assign,

        //创建
        Create,

        //开始
        Start,

        //结束
        End,

        //删除
        Delete
    }
}
