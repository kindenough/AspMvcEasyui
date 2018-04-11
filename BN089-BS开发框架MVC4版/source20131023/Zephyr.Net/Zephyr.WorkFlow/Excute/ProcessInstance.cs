using System;
using System.Collections.Generic;
using System.Collections;

namespace Zephyr.WorkFlow
{
    public class ProcessInstance
    {
      
        #region 新版
        private ProcessDefinition _processDefinition;
        private wf_processInstance _model;

        internal ProcessInstance(ProcessDefinition processDefinition,wf_processInstance model=null)
        {
            _processDefinition = processDefinition;
            _model = model;
            if (_model == null) //create new
            {
                _model = new wf_processInstance();
                //_model.ProcessDefinitionId = processDefinition.GetId();
                _model.Id = _processDefinition.getBpmContext().save(this);
            }
        }
 
        public ProcessInstance start()
        {
            _model.StartTime = DateTime.Now;
            _model.ProcessState = ProcessState.Run.ToString();

            var startTask = _processDefinition.getStartTask();
            var startTaskInstance = startTask.createTaskInstance(this);
            var rootToken = startTaskInstance.createToken();
            rootToken.signal();
            //var taskInstance = new TaskInstance
            //var startTask = _processDefinition.GetStartTask();
            //var taskInstance = new TaskInstance(this, startTask); //声明流程工作项的实例 创建流程工作项
 
            //_token = new Token(taskInstance);//声明Token的实例  创建Token
            //_token.Reactivate(true);
            //this._bpmContext.save(this);

            ////启动流程
            //_token.Signal();
            return this;
        }

        //暂停
        public ProcessInstance suspend()
        {
            _model.ProcessState = ProcessState.Pending.ToString();
            _model.EndTime = DateTime.Now;
            return this;
        }

        //恢复
        public ProcessInstance resume()
        {
            _model.ProcessState = ProcessState.Run.ToString();
            _model.EndTime = null;
            return this;
        }

        //结束流程
        public ProcessInstance end()
        {
            _model.ProcessState = ProcessState.Stop.ToString();
            _model.EndTime = DateTime.Now;
            return this;
        }

        public wf_processInstance getProcessInstanceModel()
        {
            return _model;
        }

        public ProcessDefinition getProcessDefinition()
        {
            return _processDefinition;
        }

        public BpmContext getBpmContext()
        {
            return _processDefinition.getBpmContext();
        }

        public ProcessInstance save()
        {
            _processDefinition.getBpmContext().save(this);
            return this;
        }
        #endregion


        #region 旧版本

        #region 私有
        //private string _id;
        //private string _name;
        //private Token _token;
        //private ProcessState _state;
        //private Hashtable _variable;
        //private DateTime? _startTime;
        //private DateTime? _endTime;
        //private BpmContext _bpmContext;
        //private List<Token> _temptoken;
        //private ProcessInstance _parentProcessInst;
        #endregion
        // #region 构造函数
       // //bpmConfiguration  createJbpmContext=> bpmContext loadProcessInstance=> ProcessInstance getContextInstance=>ContextInstance
       // //根据流程定义创建流程实例
       // public ProcessInstance(ProcessDefinition pd, string id, string name, Token token, ProcessState processStaste,
       //     Hashtable variable, DateTime? startTime, DateTime? endTime, BpmContext bc, ProcessInstance parentProcessInst)
       // {
       //     _processDefinition = pd;
       //     _id = id;
       //     _name = name;
       //     _token = token;
       //     _state = processStaste;
       //     _variable = variable;
       //     _startTime = startTime;
       //     _endTime = endTime;
       //     _bpmContext = bc;
       //     _temptoken = new List<Token>();
       //     _parentProcessInst = parentProcessInst;
       // }

       // //加载流程实例
       // public ProcessInstance(ProcessDefinition pd, string id, string name, string idRootToken, ProcessState processStaste,
       //     Hashtable variable, DateTime? startTime, DateTime? endTime, BpmContext bc, string idTaskInst, TokenState rootTokenState, ProcessInstance parentProcessInst)
       // {
       //     //_processDefinition = pd;
       //     //_id = id;
       //     //_name = name;
       //     //_state = processStaste;
       //     //_variable = variable;
       //     //_startTime = startTime;
       //     //_endTime = endTime;
       //     //_bpmContext = bc;
       //     //_parentProcessInst = parentProcessInst;

       //     ////加载根Token
       //     //if (idRootToken == "")
       //     //    _token = null;
       //     //else
       //     //{
       //     //    TaskInstance taskInst = _bpmContext.loadTaskInstance(idTaskInst, this);

       //     //    _token = new Token(taskInst, null, new List<Token>(), idRootToken, rootTokenState);
       //     //    _temptoken = _bpmContext.loadTempToken(this);
       //     //}
       // }

       // #endregion

       // #region 方法
       // #region 流程控制
       // //开始流程
       // public void Start()
       // {
       //     //_startTime = DateTime.Now;
       //     //_state = ProcessState.Run;
       //     //var startTask = _processDefinition.GetStartTask();
       //     //var taskInstance = new TaskInstance(this, startTask); //声明流程工作项的实例 创建流程工作项

       //     //#region 设置变量
       //     //IDictionary<string, string> ProcessVars = _processDefinition.GetProcessVars();
       //     //object o = HOLDSession.Get("SesBPMVariables");
       //     //Dictionary<string, object> VarsValue = (o == null) ? (new Dictionary<string, object>()) : (Dictionary<string, object>)o;
       //     //foreach (var i in ProcessVars)
       //     //{
       //     //    SetVariable(i.Key, (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : HOLDSession.Get("Ses" + i.Key));
       //     //}

       //     //IDictionary<string, string> TaskVars = startTask.TaskVars;
       //     //foreach (var i in TaskVars)
       //     //{
       //     //    taskInstance.SetTaskVariable(i.Key, (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : HOLDSession.Get("Ses" + i.Key));
       //     //}
       //     //#endregion

       //     //_token = new Token(taskInstance);//声明Token的实例  创建Token
       //     //_token.Reactivate(true);
       //     //this._bpmContext.save(this);

       //     ////启动流程
       //     //_token.Signal();
       // }

       // //暂停
       // public void Suspend()
       // {
       //     _state = ProcessState.Pending;
       //     _endTime = DateTime.Now;
       // }

       // //恢复
       // public void Resume()
       // {
       //     _state = ProcessState.Run;
       //     _endTime = null;
       // }

       // //删除
       // public void Delete()
       // {
       //     _state = ProcessState.Delete;
       //     _endTime = DateTime.Now;
       // }

       // //结束流程
       // public void End()
       // {
       //     _state = ProcessState.Stop;
       //     _endTime = DateTime.Now; 
       // }

       // //流程流转
       // public void Signal(string taskInstId)
       // {
       //     ////取得当前活动Token
       //     //Token currentToken = this.GetActiveToken(taskInstId);
       //     //if (currentToken == null)
       //     //{
       //     //    _bpmContext.MsgDeal("发送失败！", HOLDCore.MsgType.Waning);
       //     //    return;
       //     //}
       //     //currentToken.Signal();
       // }

       // //流程回退
       // public void BackSignal(string taskInstId)
       // {
       //     ////取得当前活动Token
       //     //Token currentToken = this.GetActiveToken(taskInstId);
       //     //if (currentToken == null)
       //     //{
       //     //    _bpmContext.MsgDeal("召回失败：任务已经下发！", HOLDCore.MsgType.Waning);
       //     //    return;
       //     //}
       //     //currentToken.BackSignal();
       // }

       // //流程召回
       // public void RecallSignal(string taskInstId)
       // {
       //     //取得当前活动Token
       //     foreach (var item in this.GetToDoToken())
       //     {
       //         if (item.GetTaskInstance().GetArrivedTransitions().Count == 1)
       //         {
       //             TransitionInstance transInst = (TransitionInstance)item.GetTaskInstance().GetArrivedTransitions()[0];
       //             if (transInst.GetFromTaskInst().GetInstanceId() == taskInstId)
       //             {
       //                 item.BackSignal();
       //                 break;
       //             }
       //         }
       //     }
       // }
       // #endregion

       // #region Token的相关动作
       // public Token CreateTempJoinToken(TaskInstance taskInstance)
       // {
       //     var temp = new Token(taskInstance);
       //     this._temptoken.Add(temp);
       //     return temp;
       // }

       // /** 
       //* 返回当前活动的TOKEN所在的节点名称 
       //* @param instId 
       //* @return 
       //*/
       // public Token GetActiveToken(string taskInstId) 
       // {
       //     Token rootToken = _token;
       //     Token currentToken = null;
       //     List<Token> tokenList = new List<Token>();
       //     List<Token> tokenTempList = new List<Token>();

       //     //添加临时token组
       //     tokenTempList.AddRange(_temptoken);
       //     tokenTempList.Add(rootToken);

       //     foreach (var item in tokenTempList)
       //     {
       //         //查找
       //         if (item.HasActivityChild())
       //         {
       //             getActivityTokenList(item, tokenList);
       //         }
       //         else if (!item.HasEnded())
       //         {
       //             tokenList.Add(item);
       //         }
       //     }

       //     foreach (var item in tokenList)
       //     {
       //         TaskInstance taskInst = item.GetTaskInstance();
       //         Task task = taskInst.GetTask();

       //         if (taskInst.GetInstanceId() == taskInstId)
       //         {
       //             if (item.GetTokenState() == TokenState.Activate)
       //             {
       //                 currentToken = item;
       //                 break;
       //             }
       //         }
       //     }

       //     return currentToken;
       // }

       // //返回待办的Tokenlist
       // public IList<Token> GetToDoToken()
       // {
       //     Token rootToken = _token;
       //     List<Token> toDoToken = new List<Token>();
       //     List<Token> tokenList = new List<Token>();
       //     List<Token> tokenTempList = new List<Token>();

       //     //添加临时token组
       //     tokenTempList.AddRange(_temptoken);
       //     tokenTempList.Add(rootToken);

       //     foreach (var item in tokenTempList)
       //     {
       //         //查找
       //         if (item.HasActivityChild())
       //         {
       //             getActivityTokenList(item, tokenList);
       //         }
       //         else if (!item.HasEnded())
       //         {
       //             tokenList.Add(item);
       //         }
       //     }

       //     foreach (var item in tokenList)
       //     {
       //         TaskInstance taskInst = item.GetTaskInstance();
       //         Task task = taskInst.GetTask();

       //         if (item.GetTokenState() == TokenState.Activate)
       //         {
       //             toDoToken.Add(item);
       //         }
       //     }

       //     return toDoToken;
       // }

       // #endregion

       // #region 私有方法
       // /** 
       // * 递归获取Token中子TOKEN所在节点的名称 
       // * @param parenToken 
       // * @param set 
       // * @return 
       // */
       // private List<Token> getActivityTokenList(Token parenToken, List<Token> set)
       // {
       //     IList<Token> tokenList = parenToken.GetActivityChild();
       //     foreach (var token in tokenList)
       //     {
       //         // 如果还有活动的子节点，说明还未全到达Join节点，需要对所有的节点的所在节点进行记录  
       //         if (token.HasActivityChild())
       //         {
       //             getActivityTokenList(token, set);
       //         }
       //         else if (!token.HasEnded())
       //         {
       //             set.Add(token);
       //         }
       //     }

       //     return set;
       // }

       // #endregion

       // #region 获取设置属性
       // //设置变量
       // public void SetVariable(string name, object value)
       // {
       //     _variable[name] = value;
       // }

       // //取得变量
       // public object GetVariable(string name)
       // {
       //     return _variable[name];
       // }

       // //取得当前令牌
       // public Token GetRootToken()
       // {
       //     return _token;
       // }

       // //取得临时Token
       // public List<Token> GetTempToken()
       // {
       //     return _temptoken;
       // }

       // //获取流程实例{IDProcessDef}-{key}
       // public string GetId()
       // {
       //     return _id;
       // }
       // public string GetName()
       // {
       //     return _name;
       // }
       // //取得流程定义
       // public ProcessDefinition GetProcessDefinition()
       // {
       //     return _processDefinition;
       // }

       // //取得父级流程实例
       // public ProcessInstance GetParentProcessInst()
       // {
       //     return _parentProcessInst;
       // }

       // //get context instance
       // public BpmContext GetContextInstance()
       // {
       //     if (_bpmContext == null)
       //     {
       //         BpmConfiguration bpmConfig = BpmConfiguration.CreateInstance();
       //         _bpmContext = bpmConfig.CreateBpmContext();
       //     }

       //     return _bpmContext;
       // }

       // //流程是否已经结束
       // public bool hasEnded()
       // {
       //     return (_state == ProcessState.Stop) ? true : false;
       // }

       // //获取到开始时间
       // public DateTime? GetStartTime()
       // {
       //     return _startTime;
       // }

       // //获取结束时间
       // public DateTime? GetEndTime()
       // {
       //     return _endTime;
       // }

       // //获取的状态
       // public ProcessState GetState()
       // {
       //     return _state;
       // }

       // #endregion

       // #endregion
        #endregion
    }
}
