using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.WorkFlow
{
    public enum ProcessState
    {
        //流程状态

        //运行
        Run,

        //停止
        Stop,

        //挂起
        Pending,

        //完成
        Complete,

        //终止
        Termination,

        //删除
        Delete
    }
}
