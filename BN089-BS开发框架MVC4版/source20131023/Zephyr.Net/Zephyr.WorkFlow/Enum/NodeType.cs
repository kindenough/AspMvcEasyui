using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.WorkFlow
{
    public enum NodeType
    {
        //开始结点
        Start,

        //任务结点
        Task,

        //结束结点
        End,

        //等待结点
        Wait,

        //发散结点
        Fork,

        //聚集结点
        Join,

        //子流程
        SubFlow,

        //决策节点
        Decision,

        //自动节点
        Auto
    }
}
