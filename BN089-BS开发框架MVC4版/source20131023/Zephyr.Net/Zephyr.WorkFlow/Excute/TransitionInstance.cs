using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace Zephyr.WorkFlow
{
    public class TransitionInstance
    {
        private TaskInstance _fromTaskInstance;
        private Transition _transition;
        private wf_transitionInstance _model;

        internal TransitionInstance(TaskInstance fromTaskInstance,Transition transition, wf_transitionInstance model=null)
        {
            _fromTaskInstance = fromTaskInstance;
            _transition = transition;
            if (_model == null)
            {
                var bpmContext = fromTaskInstance.getProcessInstance().getBpmContext();
                _model = new wf_transitionInstance();
                _model.FromTaskInstanceId = fromTaskInstance.getTaskInstanceModel().Id;
                _model.TransitionId = transition.GetId();
                _model.Id = bpmContext.save(this);
            }
        }

        public TransitionInstance setToTaskInstanceId(int toTaskInstanceId) 
        {
            _model.ToTaskInstanceId = toTaskInstanceId;
            return this;
        }

        public TransitionInstance save()
        {
            _fromTaskInstance.getProcessInstance().getBpmContext().save(this);
            return this;
        }

        public wf_transitionInstance getTransitionInstanceModel()
        {
            return _model;
        }

        public bool validate()
        {
            return true;
        }

        public TransitionInstance start()
        {
            //do ..
            return this;
        }

        public TransitionInstance end()
        {
            //do ..
            return this;
        }
 
        //运行弧线
        public void take(Token token)
        {
            this.start();

            var toTask = _transition.To;
            var processInstance = _fromTaskInstance.getProcessInstance();
            var toTaskInstance = toTask.loadOrCreateTaskInstance(processInstance);

            _model.IsExcuted = true;
            this.end().save();

            toTaskInstance.enter(this, token);

            
            //TaskInstance toTaskInstance = null;
            //var bpmContext = processInstance.getBpmContext();
            //var processInstance = _fromTaskInstance.getProcessInstance();
            //var processInstanceId = processInstance.getProcessInstanceModel().Id;

            //toTask.createTaskInstance();

            //if (toTask.NodeType == NodeType.Join)
            //    var toTaskInstance = bpmContext.loadTaskInstance(processInstanceId, toTask.Name, TaskState.Run);

            //if (toTaskInstance == null)
            //    toTaskInstance = TaskInstance.create(processInstance,toTask);
            

            //TaskInstance toTaskInst = getNextTaskInstance(processInstance, _transition.To);
            //_isExecuted = true;
            //processInstance.GetContextInstance().save(this);

            //End();

            //toTaskInst.Enter(token, this);

            //executionContext.getToken().setNode(null);
            //// pass the token to the destinationNode node
            //to.enter(executionContext);
        }



        #region 旧版本
        //public Action<TransEventArgs> OnEnter { get; set; }
        //public Action<TransEventArgs> OnLeave { get; set; }

        //private string _id;
        //private TaskInstance _fromTaskInst;
        //private bool _isExecuted;
        //private Transition _transition;

        //#region 构造函数
        ////创建
        //public TransitionInstance(Transition transition, TaskInstance fromTaskInst)
        //{
        //    //this._transition = transition;
        //    //this._fromTaskInst = fromTaskInst;
        //    //this._isExecuted = false;
        //    //this._id = fromTaskInst.GetProcessInstance().GetContextInstance().GetCustomID(InstanceType.TransitionInstance.ToString());

        //    //_initEvent();
        //}

        ////加载
        //public TransitionInstance(string id, TaskInstance fromTaskInst, bool isExecuted, Transition transition)
        //{
        //    this._transition = transition;
        //    this._fromTaskInst = fromTaskInst;
        //    this._isExecuted = isExecuted;
        //    this._id = id;

        //    _initEvent();
        //}

        //private void _initEvent()
        //{
        //    var eventMap = new Dictionary<EventType, List<ActionInstance>>();

        //    var actions = (ArrayList)_transition.Event;
        //    foreach (var i in actions)
        //    {
        //        EventAction act = i as EventAction;
        //        var actionInstance = new ActionInstance(act);
        //        if (!eventMap.ContainsKey(act.EventType))
        //            eventMap[act.EventType] = new List<ActionInstance>();

        //        eventMap[act.EventType].Add(actionInstance);
        //    }

        //    foreach (var i in eventMap)
        //    {
        //        switch (i.Key)
        //        {
        //            case EventType.Enter:
        //                this.OnEnter = (transEventArgs) => eventMap[EventType.Enter].ForEach(x => x.Fire(transEventArgs));
        //                break;
        //            case EventType.Leave:
        //                this.OnLeave = (transEventArgs) => eventMap[EventType.Leave].ForEach(x => x.Fire(transEventArgs));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        //#endregion

        //#region 方法
        //public void Start()
        //{
        //    if (OnEnter != null) OnEnter(new TransEventArgs(this));
        //}

        //public void End()
        //{
        //    if (OnLeave != null) OnLeave(new TransEventArgs(this));
        //}


        ////Instance
        //public bool Validate()
        //{
        //    var Condition = _transition.Condition;
        //    bool isTrue = true;
        //    ////根据Condtion 判断Transition是否满足条件
        //    //if (Condition == null || Condition.Trim().Length == 0)
        //    //{
        //    //    isTrue = true;
        //    //}
        //    //else
        //    //{
        //    //    //Condition.Replace()
        //    //    string s = ReplaceVariable(_fromTaskInst, Condition).Replace("'","\"");
        //    //    isTrue = Evaluator.EvaluateToBool(s); ;
        //    //}

        //    return isTrue;
        //}

        ////运行弧线
        //public void take(ProcessInstance processInstance, Token token)
        //{
        //    //Start();

        //    ////token.SetTaskInstance(null);
        //    //TaskInstance toTaskInst = getNextTaskInstance(processInstance, _transition.To);
        //    //_isExecuted = true;
        //    //processInstance.GetContextInstance().save(this);

        //    //End();

        //    //toTaskInst.Enter(token,this);

        //    ////executionContext.getToken().setNode(null);
        //    ////// pass the token to the destinationNode node
        //    ////to.enter(executionContext);
        //}

        //public void SetFromTaskInst(TaskInstance taskInst)
        //{
        //    this._fromTaskInst = taskInst;
        //}
        //#endregion

        //#region private 替换条件里的变量
        //private string ReplaceVariable(TaskInstance taskInstSource, string condition)
        //{
        //    string result = condition;
        //    //ProcessInstance processInstance = taskInstSource.GetProcessInstance();
        //    //TaskInstance fromTaskInst = taskInstSource;
        //    //IDictionary<string, object> vars = new Dictionary<string, object>();        //name , type,value

        //    //IDictionary<string, string> processVars = processInstance.GetProcessDefinition().GetProcessVars();
        //    //IDictionary<string, string> taskVars = fromTaskInst.GetTask().TaskVars;

        //    //foreach (var i in processVars)
        //    //{
        //    //    object[] o = { i.Value, processInstance.GetVariable(i.Key) };
        //    //    vars[i.Key] = o;
        //    //}

        //    //foreach (var i in taskVars)
        //    //{
        //    //    object[] o = { i.Value, fromTaskInst.GetTaskVariable(i.Key) };
        //    //    vars[i.Key] = o;
        //    //}

        //    //foreach (var item in vars)
        //    //{
        //    //    string pattern = "#{ {0,}" + item.Key + " {0,}}#";
        //    //    string replacement = getReplaceString(((object[])item.Value)[1], Convert.ToString(((object[])item.Value)[0]));      //todo根据类型进行替换
        //    //    Regex rgx = new Regex(pattern);
        //    //    result = rgx.Replace(result, replacement);
        //    //}

        //    return result;
        //}

        //private string getReplaceString(object o, string stype)
        //{
        //    string sReturn = "";

        //    switch (stype.ToLower())
        //    {
        //        case "int":
        //            sReturn = Convert.ToString(Convert.ToInt32(o));
        //            break;
        //        case "string":
        //            sReturn = string.Format(@"'{0}'", o);
        //            break;
        //        case "bool":
        //            sReturn = Convert.ToString(Convert.ToBoolean(o)).ToLower();
        //            break;
        //        default:
        //            sReturn = Convert.ToString(o);
        //            break;
        //    }

        //    return sReturn;
        //}
        //#endregion

        //private TaskInstance getNextTaskInstance(ProcessInstance processInstance, Task toTask) 
        //{
        //    //if (toTask.NodeType == NodeType.Join)
        //    //{
        //    //    //取得其判断是否存在同一来源
        //    //    //从根Token的tempToken里寻找
        //    //    var toTaskName = toTask.Name;
        //    //    var rootToken = processInstance.GetRootToken();
        //    //    var TempToken = processInstance.GetTempToken();
        //    //    LogicType logicType = toTask.LogicType;

        //    //    switch (logicType)
        //    //    {
        //    //        case LogicType.AND:         //join的Token停留在Temp等待
        //    //            foreach (var i in TempToken)
        //    //            {
        //    //                if (!i.HasEnded() && i.GetTaskInstance().GetTask().Name == toTaskName)
        //    //                    return i.GetTaskInstance();
        //    //            }
        //    //            break;
        //    //        case LogicType.OR:          //join的Token不停留Temp
        //    //            foreach (var i in TempToken)
        //    //            {
        //    //                if (i.GetTaskInstance().GetTask().Name == toTaskName)
        //    //                    return i.GetTaskInstance();

        //    //                TaskInstance taskInst = loopFormTaskInst(i.GetTaskInstance(), toTaskName);

        //    //                if (taskInst != null) return taskInst;
        //    //            }
        //    //            break;
        //    //        default: 
        //    //            break;
        //    //    }
                
        //    //}

        //    return TaskInstance.create(processInstance, _transition.To);
        //}

        //////递归寻找join或的历史TaskInst
        ////private TaskInstance loopFormTaskInst(TaskInstance CurrentTaskInst, string TargetTaskName)
        ////{
        ////    var currentTask = CurrentTaskInst.GetTask();
        ////    TaskInstance targetTaskInst = null;

        ////    if (currentTask.NodeType == NodeType.Join)              //到达Join节点了
        ////    {
        ////        if (currentTask.Name == TargetTaskName)
        ////            targetTaskInst = CurrentTaskInst;
        ////    }
        ////    else                                                    //未Join节点了
        ////    {
        ////        TaskInstance formTaskInst = ((TransitionInstance)(CurrentTaskInst.GetArrivedTransitions()[0])).GetFromTaskInst();
        ////        targetTaskInst = loopFormTaskInst(formTaskInst, TargetTaskName);
        ////    }

        ////    return targetTaskInst;

        ////}

        //#region 获取属性
        //public string GetId()
        //{
        //    return _id;
        //}

        //public TaskInstance GetFromTaskInst()
        //{
        //    return _fromTaskInst;
        //}

        //public bool GetIsExecuted()
        //{
        //    return _isExecuted;
        //}

        //public Transition GetTransition()
        //{
        //    return _transition;
        //}

        //public void SetIsExecuted(bool b)
        //{
        //    _isExecuted = b;
        //}
        //#endregion

        #endregion
    }
}
