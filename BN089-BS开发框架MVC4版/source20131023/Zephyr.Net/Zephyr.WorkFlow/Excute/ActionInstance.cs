using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.WorkFlow;
using System.Reflection;
using Zephyr.WorkFlow;

namespace Zephyr.WorkFlow
{
    public class ActionInstance
    {
        public bool isExecuted { get; set; }
        public EventAction eventAction { get; set; }

        public ActionInstance(EventAction eventAction) { 
            this.eventAction = eventAction;
        }

        public void Fire(TaskEventArgs taskEventArgs) {
            //EventHandle(taskEventArgs);
            //isExecuted = true;

            //BpmContext bpmContext = taskEventArgs.taskInstance.GetProcessInstance().GetContextInstance();
            //string sMsg = bpmContext.Msg.Message;
            //if (sMsg.Length == 0) bpmContext.Msg.Type = HOLDCore.MsgType.Success;
            //bpmContext.MsgDeal(sMsg, bpmContext.Msg.Type);
        }

        public void Fire(TransEventArgs transEventArgs)
        {
            //EventHandle(transEventArgs);
            //isExecuted = true;

            //BpmContext bpmContext = transEventArgs.taskInstance.GetProcessInstance().GetContextInstance();
            //string sMsg = bpmContext.Msg.Message;
            //if (sMsg.Length == 0) bpmContext.Msg.Type = HOLDCore.MsgType.Success;
            //bpmContext.MsgDeal(sMsg, bpmContext.Msg.Type);
        }

        private void EventHandle(TaskEventArgs taskEventArgs)
        {
            var actionType = eventAction.ActionType;
            var strCls = eventAction.Class;
            var strMethod = eventAction.Method;
            var strVars = eventAction.VarNames;

            switch (actionType)
            {
               case ActionType.Class:
                    var type = Assembly.Load(strCls.Substring(0, strCls.IndexOf('.'))).GetType(strCls);
                    var instance = Activator.CreateInstance(type);
                    var methodInfo = type.GetMethod(strMethod);

                    //var param = new object[] { taskEventArgs };   

                    #region for HOLDFrameWork
                    //将变量转换处理
                    var varType = Assembly.Load("HOLDCommon").GetType("HOLDCommon.ZephyrCommon");
                    var varInstance = Activator.CreateInstance(varType);
                    var varMethodInfo = varType.GetMethod("ExcuteTaskParam");
                    var varParam = new object[] { taskEventArgs, strVars };
                    var param = varMethodInfo.Invoke(varInstance, varParam) as object[];

                    #endregion

                    methodInfo.Invoke(instance, param);
                    break;
               case ActionType.Common:
                    strCls = GetCommonClass();
                    var typeCommon = Assembly.Load(strCls.Substring(0, strCls.IndexOf('.'))).GetType(strCls);
                    var instanceCommon = Activator.CreateInstance(typeCommon);
                    var methodInfoCommon = typeCommon.GetMethod(strMethod);
                    var paramCommon = new object[] { taskEventArgs, strVars };

                    methodInfoCommon.Invoke(instanceCommon, paramCommon);
                    break;
                default:
                    break;
            }        
        }

        private void EventHandle(TransEventArgs transEventArgs)
        {
            var actionType = eventAction.ActionType;
            var strCls = eventAction.Class;
            var strMethod = eventAction.Method;
            var strVars = eventAction.VarNames;

            switch (actionType)
            {
                case ActionType.Class:
                    var type = Assembly.Load(strCls.Substring(0, strCls.IndexOf('.'))).GetType(strCls);
                    var instance = Activator.CreateInstance(type);
                    var methodInfo = type.GetMethod(strMethod);

                    //var param = new object[] { taskEventArgs };   

                    #region for HOLDFrameWork
                    //将变量转换处理
                    var varType = Assembly.Load("HOLDCommon").GetType("HOLDCommon.ZephyrCommon");
                    var varInstance = Activator.CreateInstance(varType);
                    var varMethodInfo = varType.GetMethod("ExcuteTransParam");
                    var varParam = new object[] { transEventArgs, strVars };
                    var param = varMethodInfo.Invoke(varInstance, varParam) as object[];

                    #endregion

                    methodInfo.Invoke(instance, param);
                    break;
                case ActionType.Common:
                    strCls = GetCommonClass();
                    var typeCommon = Assembly.Load(strCls.Substring(0, strCls.IndexOf('.'))).GetType(strCls);
                    var instanceCommon = Activator.CreateInstance(typeCommon);
                    var methodInfoCommon = typeCommon.GetMethod(strMethod);
                    var paramCommon = new object[] { transEventArgs };

                    methodInfoCommon.Invoke(instanceCommon, paramCommon);
                    break;
                default:
                    break;
            }
        }

        //从配置文件取得Common类的名字
        private string GetCommonClass()
        {
            //var strCls = HOLDUtils.HOLDConfig.GetConfigString("BPMCommonClass");
            //return strCls;
            return "";
        }

        public EventType GetEnvetType() {
            return eventAction.EventType;
        }


    }
}
