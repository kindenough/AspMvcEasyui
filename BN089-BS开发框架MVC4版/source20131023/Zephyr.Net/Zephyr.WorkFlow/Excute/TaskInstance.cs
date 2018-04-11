using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;

namespace Zephyr.WorkFlow
{
    public class TaskInstance
    {
 
        private ProcessInstance _processInstance;
        private Task _task;
        private wf_taskInstance _model;
      
        internal TaskInstance(ProcessInstance processInstance, Task task, wf_taskInstance model = null)
        {
            _processInstance = processInstance;
            _task = task;
            if (_model == null)
            {
                var bpmContext = processInstance.getBpmContext();
                _model = new wf_taskInstance();
                _model.ProcessInstanceId = processInstance.getProcessInstanceModel().Id;
                _model.Id = bpmContext.save(this);
            }
        }

        public Token createToken()
        {
            return new Token(this, null);
        }
 
        public Token loadToken(wf_token model)
        {
            return new Token(this, model);
        }
 
        public TaskInstance start()
        {
            //if (OnEnter != null) OnEnter();
            //_startTime = DateTime.Now;
            return this;
        }

        public TaskInstance enterNode()
        {
            //if (OnEnter != null) OnEnter(new TaskEventArgs(this));
            return this;
        }


        public TaskInstance end()
        {
            //if (OnLeave != null) OnLeave();
            //_endTime = DateTime.Now;
            return this;
        }

        public TaskInstance save()
        {
            var bpmContext = _processInstance.getBpmContext();
            bpmContext.save(this);
            return this;
        }

        //进入节点
        public void enter(TransitionInstance transitionInstance,Token token)
        {
            //当工作项实体已完成并且结点为join且逻辑为or时，不再启动
            if (!(_task.NodeType == NodeType.Join
                && _task.LogicType == LogicType.OR
                && _model.TaskState == TaskState.Complete.ToString()))
                this.start();

            //更新线实例的到达工作项实例id
            transitionInstance.setToTaskInstanceId(_model.Id).save();
 
            //设置令牌的工作项实例
            token.setTaskInstance(this).save();

            this.enterNode();

            execute(token);

            //Token token = executionContext.getToken();
            //token.setNode(this);
            //// remove the transition references from the runtime context
            //executionContext.setTransition(null);
            //executionContext.setTransitionSource(null);

            //// execute the node
            //if (isAsync)
            //{

            //}
            //else
            //{
            //    execute(executionContext);
            //}
        }

        //执行节点
        public void execute(Token token)
        {
            var transitions = _task.LeavingTransitions;
            var bpmContext = _processInstance.getBpmContext();

            switch (_task.NodeType)
            {
                case NodeType.Start:
                    break;
                case NodeType.Task:
                    //IsReturnParent(token.GetTaskInstance());
                    break;
                case NodeType.Fork:
                    token.activate(false);

                    var childs = new Dictionary<TransitionInstance,Token>();
                    foreach (Transition transition in transitions)
                    {
                        var transitionsInstance = transition.createTransitionInstance(this);
                        childs.Add(transitionsInstance, this.createToken().setParent(token).activate(true).save());
                    }

                    foreach (var child in childs)
                        this.leave(child.Key, child.Value);

                    token.save();
                    break;
                case NodeType.Join://属于自动结点，外部不需要调用signal

                    var dataPackageToken = bpmContext.loadDataTokenActiveByTaskInstanceId(_model.Id);
                    var packageToken = loadToken(dataPackageToken).activate(true);  //创建或加载并激活package令牌

                    if (_model.TaskState == TaskState.Complete.ToString())//如果流程已往下走，还有线未到达的情况
                    {
                        token.setTaskInstance(this).setPackage(packageToken).end().save();
                        return;
                    }
 
                    token.activate(false).setTaskInstance(this).setPackage(packageToken).save();  //设置当前token未激活

                    //处理聚合关系
                    var packageId = packageToken.getTokenModel().Id;
                    var dataJoinParentTokens = bpmContext.loadDataJoinParentTokensByPackageId(packageId);
                    while (dataJoinParentTokens.Count() > 0)
                    {
                        foreach (var item in dataJoinParentTokens)
                        {
                            var parent = this.loadToken(item).setTaskInstance(this).setPackage(packageToken).save();
                            bpmContext.updateTokenStateByParentId(parent.getTokenModel().Id, TokenState.Ended);
                        }
                        dataJoinParentTokens = bpmContext.loadDataJoinParentTokensByPackageId(packageId); 
                    }

                    //根据逻辑关系判断是否往下流转还是等待
                    switch (_task.LogicType)
                    {
                        case LogicType.AND:
                            //所有的线都已到达
                            var arrivedTransitionIds = bpmContext.loadDataArrivedTransitionInstance(_model.Id).Select(x => x.TransitionId);
                            var arrivingTransitionIds = _task.ArrivingTransitions.OfType<Transition>().Select(x => x.GetId());
                            if (arrivingTransitionIds.Except(arrivedTransitionIds).Count() == 0)
                            {
                                packageToken.signal();
                            }
                            break;
                        case LogicType.OR:
                            packageToken.signal();
                            break;

                        case LogicType.XOR:
                            // to do
                            break;

                        case LogicType.DISC:
                            // to do
                            break;
                    }
                    break;
                //case NodeType.Wait:
                //    break;
                //case NodeType.End:
                //    token.GetTaskInstance().End();
                //    token.End();
                //    _processInstance.End();

                //    bpmContext.save(token.GetTaskInstance());
                //    bpmContext.save(token);
                //    bpmContext.save(_processInstance);

                //    IsReturnParent(token.GetTaskInstance());
                //    break;
                //case NodeType.Decision:
                //    token.Signal();
                //    break;
                //case NodeType.Auto:
                //    token.Signal();
                //    break;
                //case NodeType.SubFlow:
                //    if (this.GetTask().IsAutoStart)
                //    {
                //        ProcessInstance piStart = bpmContext.newProcessInstance(this.GetTask().SubFlowName, _processInstance.GetId());    //新增流程实例
                //        piStart.Start();
                //        bpmContext.save(piStart);
                //    }
                //    break;
                default:
                    break;
            }
        }
         
        //离开节点
        public TaskInstance leave(TransitionInstance transitionInstance, Token token)
        {
            this.end().save();
            transitionInstance.take(token);
            return this;
        }

        public wf_taskInstance getTaskInstanceModel()
        {
            return _model;
        }

        public ProcessInstance getProcessInstance()
        {
            return _processInstance;
        }

        public Task getTask()
        {
            return _task;
        }



        #region 旧版本
        //public Action<TaskEventArgs> OnEnter { get; set; }
        //public Action<TaskEventArgs> OnLeave { get; set; }
        //public Action<TaskEventArgs> OnAssign { get; set; }
        //public Action<TaskEventArgs> OnCreate { get; set; }
        //public Action<TaskEventArgs> OnStart { get; set; }
        //public Action<TaskEventArgs> OnEnd { get; set; }
        //public Action<TaskEventArgs> OnDelete { get; set; }

        //#region 私有
        //private ProcessInstance _processInstance;
        //private Task _task;
        //private string _id;
        //private DateTime? _createTime;
        //private DateTime? _startTime;
        //private DateTime? _endTime;
        //private TaskState _taskState;
        //private int _actorID;
        //private Dictionary<int, string[]> _pooledActors;
        //private Hashtable _variable;
        //private string _arrivedTransInstances;

        //#endregion

        //#region 构造函数
        //public TaskInstance(ProcessInstance processInstance, Task task, string id, DateTime? createTime, DateTime? startTime,
        //    DateTime? endTime, TaskState taskState, int actorID, Dictionary<int, string[]> pooledActors, Hashtable variable, string arrivedTransInstancesId)
        //{
        //    _processInstance = processInstance;
        //    _task = task;
        //    _id = id;
        //    _createTime = createTime;
        //    _startTime = startTime;
        //    _endTime = endTime;
        //    _taskState = taskState;
        //    _actorID = actorID;
        //    _pooledActors = pooledActors;
        //    _variable = variable;
        //    _arrivedTransInstances = arrivedTransInstancesId;

        //    _initEvent();
        //}

        ////创建的工作项声明实例
        //public TaskInstance(ProcessInstance processInstance, Task task)
        //{
            
        //}

      

        //#endregion

        //#region 方法

        //#region 任务实例操作
        //public void EnterNode()
        //{
        //    if (OnEnter != null) OnEnter(new TaskEventArgs(this));
        //}

        //public void Start()
        //{
        //    //_startTime = DateTime.Now;
        //    //_taskState = TaskState.Run;
        //    //if (OnStart != null) OnStart(new TaskEventArgs(this));
        //}

        //public void End()
        //{
        //    //if (OnLeave != null) OnLeave(new TaskEventArgs(this));
        //    //_endTime = DateTime.Now;
        //    //_actorID = _processInstance.GetContextInstance().CurrentUser.IDUser;
        //    //_taskState = TaskState.Complete;
        //}

        //public void Delete()
        //{
        //    //_taskState = TaskState.Delete;
        //    //_arrivedTransInstances = "";
        //    //_actorID = _processInstance.GetContextInstance().CurrentUser.IDUser;
        //    //if (OnDelete != null) OnDelete(new TaskEventArgs(this));
        //}

        ////进入节点
        //public void Enter(Token token, TransitionInstance transitionInstance)
        //{
        //    ////增加判断当前TaskInst为完成时
        //    //if (!(this.GetInstanceStatus() == TaskState.Complete && this.GetTask().NodeType == NodeType.Join && this.GetTask().LogicType == LogicType.OR))
        //    //    this.Start();

        //    //_arrivedTransInstances += "," + transitionInstance.GetId();
        //    //_arrivedTransInstances = _arrivedTransInstances.Trim(',');
        //    //_processInstance.GetContextInstance().save(this);

        //    //token.SetTaskInstance(this);

        //    //this.EnterNode();

        //    //_processInstance.GetContextInstance().save(token);

        //    //Execute(token);

        //    ////Token token = executionContext.getToken();
        //    ////token.setNode(this);
        //    ////// remove the transition references from the runtime context
        //    ////executionContext.setTransition(null);
        //    ////executionContext.setTransitionSource(null);

        //    ////// execute the node
        //    ////if (isAsync)
        //    ////{

        //    ////}
        //    ////else
        //    ////{
        //    ////    execute(executionContext);
        //    ////}
        //}

        ////离开节点
        //public void Leave(Token token, TransitionInstance transitionInstance)
        //{
        //    //this.End();
        //    //var bpmContext = _processInstance.GetContextInstance();
        //    //bpmContext.save(this);
        //    ////token.SetTaskInstance(this);

        //    //if (transitionInstance == null) return;
        //    //transitionInstance.take(_processInstance, token);

        //    ////事例
        //    ////Token token = executionContext.getToken();
        //    ////token.setNode(this);
        //    ////executionContext.setTransition(transition);
        //    ////executionContext.setTransitionSource(this);
        //    ////transition.take(executionContext);
        //}

        ////执行节点
        //public void Execute(Token token)
        //{
        //    //NodeType nodeType = token.GetTaskInstance().GetTask().NodeType;
        //    //var task = token.GetTaskInstance().GetTask();
        //    //var transitions = task.LeavingTransitions;
        //    //var bpmContext = _processInstance.GetContextInstance();

        //    //switch (nodeType)
        //    //{
        //    //    case NodeType.Start:
        //    //        break;
        //    //    case NodeType.Task:
        //    //        IsReturnParent(token.GetTaskInstance());
        //    //        break;
        //    //    case NodeType.Fork:
        //    //        var taskInstSource = token.GetTaskInstance();
        //    //        Dictionary<Token, TransitionInstance> childs = new Dictionary<Token, TransitionInstance>();
        //    //        token.Reactivate(false);

        //    //        #region 设置工作项变量
        //    //        IDictionary<string, string> taskVars = task.TaskVars;
        //    //        object o = HOLDSession.Get("SesBPMVariables");
        //    //        Dictionary<string, object> VarsValue = (o == null) ? (new Dictionary<string, object>()) : (Dictionary<string, object>)o;
        //    //        foreach (var i in taskVars)
        //    //        {
        //    //            taskInstSource.SetTaskVariable(i.Key, (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : HOLDSession.Get("Ses" + i.Key));
        //    //        }
        //    //        #endregion

        //    //        foreach (Transition item in transitions)
        //    //        {
        //    //            Token childToken = new Token(taskInstSource, token);
        //    //            childToken.Reactivate(true);
        //    //            token.GetChild().Add(childToken);
        //    //            var transitionInst = new TransitionInstance(item, taskInstSource);

        //    //            childs.Add(childToken, transitionInst);
        //    //        }

        //    //        foreach (var item in childs)
        //    //        {
        //    //            Token childToken = item.Key;
        //    //            TransitionInstance transInst = item.Value;

        //    //            taskInstSource.Leave(childToken, transInst);
        //    //        }

        //    //        bpmContext.save(token);
        //    //        //mod by yrh 20130312 for code clean
        //    //        //foreach (var item in transitions)
        //    //        //{
        //    //        //    Transition transition = (Transition)item;
        //    //        //    Task toTask = transition.To;
        //    //        //    TaskInstance taskInstChild = new TaskInstance(_processInstance, toTask);
        //    //        //    Token childToken = new Token(taskInstChild, token);
        //    //        //    childToken.Reactivate(true);
        //    //        //    token.GetChild().Add(childToken);
        //    //        //    var transitionInst = new TransitionInstance(transition, taskInstSource);

        //    //        //    bpmContext.save(taskInstChild);
        //    //        //    bpmContext.save(childToken);

        //    //        //    childs.Add(childToken, transitionInst);
        //    //        //}

        //    //        //foreach (var item in childs)
        //    //        //{
        //    //        //    Token childToken = item.Key;
        //    //        //    TransitionInstance transitionInst = item.Value;
        //    //        //    TaskInstance taskInstChild = childToken.GetTaskInstance();

        //    //        //    taskInstChild.Enter(childToken, transitionInst);
        //    //        //    transitionInst.SetIsExecuted(true);

        //    //        //    bpmContext.save(transitionInst);
        //    //        //}
        //    //        break;
        //    //    case NodeType.Join://属于自动结点，外部不需要调用signal
        //    //        if (_taskState == TaskState.Complete) //如果流程已往下走，还有线未到达的情况
        //    //        {
        //    //            token.End();
        //    //            bpmContext.save(token);
        //    //            return;
        //    //        }

        //    //        token.Reactivate(false);        //设置当前token未激活

        //    //        //获取join结点的临时token
        //    //        var joinToken = _processInstance.GetActiveToken(_id);
        //    //        if (joinToken == null)
        //    //        {
        //    //            joinToken = _processInstance.CreateTempJoinToken(this);
        //    //            bpmContext.saveTempToken(_processInstance, joinToken);
        //    //        }

        //    //        //激活临时结点
        //    //        token.SetTaskInstance(joinToken.GetTaskInstance());
        //    //        bpmContext.save(token);

        //    //        joinToken.AddJoinTokenMap(token);//生成parent=>tokens关系表
        //    //        joinToken.Reactivate(true);

        //    //        //递归聚合处理
        //    //        joinToken = joinTokenHandle(joinToken);

        //    //        //根据逻辑关系判断是否往下流转还是等待
        //    //        var logic = this.GetTask().LogicType;
        //    //        switch (logic)
        //    //        {
        //    //            case LogicType.AND:
        //    //                //所有的线都已到达
        //    //                var arrivedTransIds = this.GetArrivedTransitions().OfType<TransitionInstance>().Select(x => x.GetTransition().GetId());
        //    //                var arrivingTransIds = _task.ArrivingTransitions.OfType<Transition>().Select(x=>x.GetId());
        //    //                if (arrivingTransIds.Except(arrivedTransIds).Count() == 0)
        //    //                {
        //    //                    joinToken.Signal();
        //    //                }
        //    //                break;
        //    //            case LogicType.OR:
        //    //                joinToken.Signal();
        //    //                break;

        //    //            case LogicType.XOR:
        //    //                // to do
        //    //                break;

        //    //            case LogicType.DISC:
        //    //                // to do
        //    //                break;
        //    //        }
        //    //        break;
        //    //    case NodeType.Wait:
        //    //        break;
        //    //    case NodeType.End:
        //    //        token.GetTaskInstance().End();
        //    //        token.End();
        //    //        _processInstance.End();

        //    //        bpmContext.save(token.GetTaskInstance());
        //    //        bpmContext.save(token);
        //    //        bpmContext.save(_processInstance);

        //    //        IsReturnParent(token.GetTaskInstance());
        //    //        break;
        //    //    case NodeType.Decision:
        //    //        token.Signal();
        //    //        break;
        //    //    case NodeType.Auto:
        //    //        token.Signal();
        //    //        break;
        //    //    case NodeType.SubFlow:
        //    //        if (this.GetTask().IsAutoStart)
        //    //        {
        //    //            ProcessInstance piStart = bpmContext.newProcessInstance(this.GetTask().SubFlowName, _processInstance.GetId());    //新增流程实例
        //    //            piStart.Start();
        //    //            bpmContext.save(piStart);
        //    //        }
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //}

        ////复制对象
        //public TaskInstance CopyTaskInst()
        //{ 
        //    //this._id = this._processInstance.GetContextInstance().GetCustomID(HOLDConvert.ToString(InstanceType.TaskInstance));
        //    //this._actorID = 0;
        //    //this._taskState = TaskState.Run;

        //    return this;
        //}

        //#endregion

        //#region 属性获取
        ////取得工作对象
        //public Task GetTask()
        //{
        //    return _task;
        //}

        ////取得工作项实例ID
        //public string GetInstanceId()
        //{
        //    return _id;
        //}

        ////到达弧线
        //public string GetArrivedTransIds()
        //{
        //    return _arrivedTransInstances;
        //}

        ////获取当前工作项的状态
        //public TaskState GetInstanceStatus()
        //{
        //    return _taskState;
        //}

        ////取到实例Key
        //public string GetInstanceBizKey()
        //{
        //    return string.Empty;
        //}

        ////取到工作项完成人
        //public int GetInstanceActors()
        //{
        //    return _actorID;
        //}

        ////取得任务的参与人
        //public Dictionary<int, string[]> GetPooledActors()
        //{
        //    return _pooledActors;
        //}

        ////设置工作项参与人
        //public void SetInstanceActors(int actors)
        //{
        //    _actorID = actors;
        //}

        ////取到工作项创建时间
        //public DateTime? GetInstanceCreateTime()
        //{
        //    return _createTime;
        //}

        ////取到工作项开始时间
        //public DateTime? GetInstanceStartTime()
        //{
        //    return _startTime;
        //}

        ////取到工作项结束时间
        //public DateTime? GetInstanceEndTime()
        //{
        //    return _endTime;
        //}

        ////取到工作项来源
        //public string GetInstanceFormValue()
        //{
        //    return string.Empty;
        //}

        ////取到流程实例
        //public ProcessInstance GetProcessInstance()
        //{
        //    return _processInstance;
        //}

        ////设置任务变量
        //public void SetTaskVariable(string name, object value)
        //{
        //    _variable[name] = value;
        //}

        ////取到任务变量
        //public object GetTaskVariable(string name)
        //{
        //    return _variable[name];
        //}

        //public bool HasEnded()
        //{
        //    return (_taskState == TaskState.Stop) ? true : false;
        //}

        ////设置TaskInst的状态
        //public void SetTaskInstState(TaskState taskState)
        //{
        //    this._taskState = taskState;
        //}

        ////取得到达弧线实例组
        //public IList GetArrivedTransitions()
        //{
        //    IList ArrivedTransInsts = new List<TransitionInstance>();
        //    //BpmContext bpmContext = this.GetProcessInstance().GetContextInstance();

        //    //foreach (string sid in _arrivedTransInstances.Split(new Char[] { ',' }))
        //    //{
        //    //    ArrivedTransInsts.Add(bpmContext.loadTransitionInstance(sid,this._processInstance));
        //    //}

        //    return ArrivedTransInsts;
        //}
        //#endregion

        //#region 私有方法
        //private Token joinTokenHandle(Token joinToken)
        //{
        //    //var tokenDict = joinToken.GetJoinTokenMap();
        //    //var bpmContext = _processInstance.GetContextInstance();

        //    //foreach (var item in tokenDict)
        //    //{
        //    //    var parent = item.Key;
        //    //    var arrive = item.Value;
        //    //    var childs = parent.GetChild();

        //    //    //此parentToken下所有的token都已到达
        //    //    if (childs.Select(x=>x.GetId()).Except(arrive.Select(x=>x.GetId())).Count() == 0) //需要测试
        //    //    {
        //    //        arrive.ForEach(token =>
        //    //        {
        //    //            token.End();
        //    //            bpmContext.save(token);
        //    //        });

        //    //        tokenDict.Remove(parent);
        //    //        joinToken.AddJoinTokenMap(parent);

        //    //        parent.SetTaskInstance(this);

        //    //        //如果到达root结点
        //    //        if (tokenDict.Count == 0)
        //    //        {
        //    //            joinToken.End();
        //    //            bpmContext.save(joinToken);

        //    //            parent.Reactivate(true);
        //    //            bpmContext.save(parent);

        //    //            return parent;
        //    //        }

        //    //        joinToken = joinTokenHandle(joinToken);
        //    //        break;
        //    //    }
        //    //}

        //    //bpmContext.save(joinToken);
        //    //return joinToken;
        //    return null;
        //}

        //#region 是否返回父流程：父流程存在且该工作项是返回工作项
        //private void IsReturnParent(TaskInstance taskInst)
        //{
        //    //ProcessInstance CurrentProcess = taskInst.GetProcessInstance();
        //    //ProcessInstance ParentProcess = CurrentProcess.GetParentProcessInst();
        //    //Task task = taskInst.GetTask();

        //    //if (ParentProcess != null && task.ReturnParent)
        //    //{
        //    //    foreach (Token i in ParentProcess.GetToDoToken())
        //    //    {
        //    //        TaskInstance subFlowTaskInst = i.GetTaskInstance();
        //    //        if (subFlowTaskInst.GetTask().NodeType == NodeType.SubFlow && subFlowTaskInst.GetTask().SubFlowName == CurrentProcess.GetName())
        //    //            i.Signal();
        //    //    }
        //    //}
        //}
        //#endregion
        //#endregion
        //#endregion

        #endregion
    }
}
