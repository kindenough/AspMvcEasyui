using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.WorkFlow
{
    public class EunmConvert
    {
        //操作类型
        public static ProcessState ToProcessState(object objOpt)
        {
            ProcessState processState = ProcessState.Run;
            try
            {
                processState = (ProcessState)System.Enum.Parse(typeof(ProcessState), Convert.ToString(objOpt), true);
            }
            catch { }
            return processState;
        }

        //表单类型
        public static FormType ToFormType(object objOpt)
        {
            FormType formType = FormType.Url;
            try
            {
                formType = (FormType)System.Enum.Parse(typeof(FormType), Convert.ToString(objOpt), true);
            }
            catch { }
            return formType;
        }


        //节点类型
        public static NodeType ToNodeType(object objOpt)
        {
            NodeType nodetype = NodeType.Task;
            try
            {
                nodetype = (NodeType)System.Enum.Parse(typeof(NodeType), Convert.ToString(objOpt), true);
            }
            catch { }
            return nodetype;
        }

        //节点状态
        public static TaskState ToTaskState(object objOpt)
        {
            TaskState taskState = TaskState.Run;
            try
            {
                taskState = (TaskState)System.Enum.Parse(typeof(TaskState), Convert.ToString(objOpt), true);
            }
            catch { }
            return taskState;
        }

        //事件类型
        public static EventType ToEventType(object objOpt)
        {
            EventType eventType = EventType.Enter;
            try
            {
                eventType = (EventType)System.Enum.Parse(typeof(EventType), Convert.ToString(objOpt), true);
            }
            catch { }
            return eventType;
        }

        //事件分类
        public static ActionType ToActionType(object objOpt)
        {
            ActionType actionType = ActionType.Class;
            try
            {
                actionType = (ActionType)System.Enum.Parse(typeof(ActionType), Convert.ToString(objOpt), true);
            }
            catch { }
            return actionType;
        }

        //TokenState
        public static TokenState ToTokenState(object objOpt)
        {
            TokenState tokenState = TokenState.Notactive;
            try
            {
                tokenState = (TokenState)System.Enum.Parse(typeof(TokenState), Convert.ToString(objOpt), true);
            }
            catch { }
            return tokenState;
        }

        //MsgKind
        public static MsgState ToMsgState(object objOpt)
        {
            MsgState msgState = MsgState.Close;
            try
            {
                msgState = (MsgState)System.Enum.Parse(typeof(MsgState), Convert.ToString(objOpt), true);
            }
            catch { }
            return msgState;

        }

        //AlertType
        public static AlertType ToAlertType(object objOpt)
        {
            AlertType alertType = AlertType.none;
            try
            {
                alertType = (AlertType)System.Enum.Parse(typeof(AlertType), Convert.ToString(objOpt), true);
            }
            catch { }
            return alertType;
        }

        public static LogicType ToLogicType(object objOpt)
        {
            LogicType logicType = LogicType.AND;
            try
            {
                logicType = (LogicType)System.Enum.Parse(typeof(LogicType), Convert.ToString(objOpt), true);
            }
            catch { }
            return logicType;
        }
    }
}
