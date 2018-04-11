using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Zephyr.WorkFlow
{
    public class Token
    {
        private TaskInstance _taskInstance;
        private wf_token _model;

        internal Token(TaskInstance taskInstance, wf_token model = null)
        {
            _taskInstance = taskInstance;
            _model = model;
            if (_model == null)
            {
                var bpmContext = taskInstance.getProcessInstance().getBpmContext();
                _model = new wf_token();
                _model.TaskInstanceId = taskInstance.getTaskInstanceModel().Id;
                _model.ProcessInstanceId = taskInstance.getProcessInstance().getProcessInstanceModel().Id; //冗余保存
                _model.Id = bpmContext.save(this);
            }
        }

        public Token save()
        {
            _taskInstance.getProcessInstance().getBpmContext().save(this);
            return this;
        }

        //停止令牌,消亡
        public Token end()
        {
            _model.TokenState = TokenState.Ended.ToString();
            return this;
        }

        public Token setParent(Token parentToken)
        {
            _model.ParentId = parentToken.getTokenModel().Id;
            return this;
        }

        public Token setPackage(Token packageToken)
        {
            _model.PackageId = packageToken.getTokenModel().Id;
            return this;
        }

        //设置Token对应的流程实例，并保存
        public Token setTaskInstance(TaskInstance taskInstance)
        {
            _taskInstance = taskInstance;
            _model.ProcessInstanceId = _taskInstance.getTaskInstanceModel().Id;
            return this;
        }

        public Token activate(bool b)
        {
            var tokenState = b ? TokenState.Activate : TokenState.Notactive;
            _model.TokenState = tokenState.ToString();
            return this;
        }
 
        public void signal()
        {
            var processInstance = _taskInstance.getProcessInstance();
            var bpmContext = processInstance.getBpmContext();
            var task = _taskInstance.getTask();
            var transitions = task.LeavingTransitions;

            if (transitions.Count == 0)
            {
                _taskInstance.end().save();
                return;
            }

            //每次只走一条transition
            int excuteTransitionCount = 0;
            foreach (Transition transition in transitions)
            {
                //Token的Signal操作表示：实例需要离开当前token所在的节点，转移到下一个节点上。
                //因为Node与Node之间是“Transition”这个桥梁，
                //所以，在转移过程中，会首先把Token放入相关连的Transtion对象中，
                //再由Transition对象把Token交给下一个节点。 
                var transitionInstance = transition.createTransitionInstance(_taskInstance);
                if (excuteTransitionCount == 0 && transitionInstance.validate())
                {
                    excuteTransitionCount++;
                    _taskInstance.leave(transitionInstance,this);
                }
            }

            //没有线满足时，默认无条件走第一条
            if (excuteTransitionCount == 0)
            {
                var firstTransition = (Transition)transitions[0];
                var transitionInstance = firstTransition.createTransitionInstance(_taskInstance);
                _taskInstance.leave(transitionInstance, this);
            }
        }

        public wf_token getTokenModel()
        {
            return _model;
        }

        public TaskInstance getTaskInstance() 
        {
            return _taskInstance;
        }
        #region 旧版本
        //#region 私有
        //private TaskInstance _current;
        //private Token _parent;
        //private List<Token> _child;
        //private string _id;

        //private Dictionary<Token, List<Token>> _maptoken; //parent,arriveTokenList
        //private TokenState _tokenState;
        //#endregion

        //#region 构造函数
        //public Token(TaskInstance taskInstace, Token parent, List<Token> child, string id, TokenState tokenState)
        //{
        //    //_current = current;
        //    //_parent = parent;
        //    //_child = child;
        //    //_id = id;
        //    //_tokenState = tokenState;

        //    //current.GetProcessInstance().GetContextInstance().loopToken(this, _child);
        //    //_maptoken = current.GetProcessInstance().GetContextInstance().loadMapToken(id, current.GetProcessInstance());
        //}

        ////创建Token
        //public Token(TaskInstance task)
        //{
        //    //_current = task;
        //    //_id = task.GetProcessInstance().GetContextInstance().GetCustomID(InstanceType.Token.ToString());
        //    //_parent = null;
        //    //_child = new List<Token>();

        //    //_tokenState = TokenState.Notactive;
        //    //_maptoken = new Dictionary<Token, List<Token>>();
        //}

        //public Token(TaskInstance task, Token parent)
        //{
        //    //_current = task;
        //    //_id = task.GetProcessInstance().GetContextInstance().GetCustomID(InstanceType.Token.ToString());
        //    //_parent = parent;
        //    //_child = new List<Token>();

        //    //_tokenState = TokenState.Notactive;
        //    //_maptoken = new Dictionary<Token, List<Token>>();
        //}

        //#endregion

        //#region 方法
        //#region Token操作
        ////Token流转
        //public void Signal()
        //{
        //    //var processInstance = _current.GetProcessInstance();
        //    //var taskInstSource = _current;
        //    //var task = taskInstSource.GetTask();
        //    //BpmContext bpmContext = processInstance.GetContextInstance();

        //    //#region 设置工作项变量
        //    //IDictionary<string, string> taskVars = task.TaskVars;
        //    //object o = HOLDSession.Get("SesBPMVariables");
        //    //Dictionary<string, object> VarsValue = (o == null) ? (new Dictionary<string, object>()) : (Dictionary<string, object>)o;
        //    //foreach (var i in taskVars)
        //    //{
        //    //    _current.SetTaskVariable(i.Key, (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : HOLDSession.Get("Ses" + i.Key));
        //    //}
        //    //#endregion

        //    //var transitions = task.LeavingTransitions;

        //    ////每次只走一条transition
        //    //int ExcuteTransCount = 0;
        //    //var firstTransition = (transitions.Count > 0) ? (Transition)transitions[0] : null;

        //    //foreach (Transition item in transitions)
        //    //{
        //    //    var transitionInst = new TransitionInstance(item, taskInstSource);

        //    //    //Token的Signal操作表示：实例需要离开当前token所在的节点，转移到下一个节点上。
        //    //    //因为Node与Node之间是“Transition”这个桥梁，
        //    //    //所以，在转移过程中，会首先把Token放入相关连的Transtion对象中，
        //    //    //再由Transition对象把Token交给下一个节点。 
        //    //    if (ExcuteTransCount == 0 && transitionInst.Validate())
        //    //    {
        //    //        ExcuteTransCount++;
        //    //        _current.Leave(this, transitionInst);
        //    //    }
        //    //}

        //    ////没有线满足时，默认走第一条
        //    //if (ExcuteTransCount == 0)
        //    //{
        //    //    var transitionInst = (firstTransition == null) ? null : new TransitionInstance(firstTransition, taskInstSource);
        //    //    if (firstTransition == null) { this.End(); bpmContext.save(this); }
        //    //    _current.Leave(this, transitionInst);
        //    //}
        //}

        ////Token回退--上一节点为Task节点时才进行回退
        //public void BackSignal()
        //{
        //    //IList arriveTrans = _current.GetArrivedTransitions();

        //    //if (arriveTrans.Count == 1)
        //    //{ 
        //    //    TransitionInstance transInst = (TransitionInstance)arriveTrans[0];
        //    //    var fromTaskInst = transInst.GetFromTaskInst();
        //    //    var fromTask = fromTaskInst.GetTask();

        //    //    if (fromTask.NodeType == NodeType.Task)
        //    //    {
        //    //        ProcessInstance pi = _current.GetProcessInstance();
        //    //        BpmContext bpmContext = pi.GetContextInstance();

        //    //        //当前的工作项状态设为删除,将Token的工作项设置成来源工作项
        //    //        _current.Delete();          //1.删除当前待办节点
        //    //        bpmContext.save(_current);
        //    //        transInst.SetFromTaskInst(null);
        //    //        bpmContext.save(transInst); //2.删除来源线

        //    //        fromTaskInst.SetTaskInstState(TaskState.Run);//3.设置来源节点为可运行状态
        //    //        this.SetTaskInstance(fromTaskInst);//4.token设置为来源工作项

        //    //        bpmContext.save(fromTaskInst);
        //    //        bpmContext.save(this);
        //    //    }
        //    //}
        //}

        ////停止令牌,消亡
        //public void End()
        //{
        //    _tokenState = TokenState.Ended;
        //}

        //public void Reactivate(bool b)
        //{
        //    _tokenState = b ? TokenState.Activate : TokenState.Notactive;
        //}

        //public void AddJoinTokenMap(Token token)
        //{
        //    //Join临时token到达时
        //    var tokenMap = token.GetJoinTokenMap();
        //    if (tokenMap.Count > 0)
        //    {
        //        foreach (var item in tokenMap)
        //        {
        //            var key = item.Key;
        //            if (!_maptoken.ContainsKey(key))
        //                _maptoken[key] = new List<Token>();

        //            _maptoken[key].AddRange(item.Value);
        //        }
        //        return;
        //    }

        //    //其它普通token到达时
        //    var parent = token.GetParent();
        //    if (parent != null)
        //    {
        //        if (!_maptoken.ContainsKey(parent))
        //            _maptoken[parent] = new List<Token>();

        //        _maptoken[parent].Add(token);
        //    }
        //}

        ////设置Token对应的流程实例，并保存
        //public void SetTaskInstance(TaskInstance taskInstance)
        //{
        //    ////1.设置current
        //    ////2.处理Parent,child
        //    //_current = taskInstance;
        //    //if (_current == null) return;
        //    //ProcessInstance pi = _current.GetProcessInstance();

        //    ////保存Token
        //    //BpmContext bc = pi.GetContextInstance();
        //    //bc.save(this);
        //}
        //#endregion

        //#region 属性获取
        //public string GetId()
        //{
        //    return _id;
        //}

        ////取得Join节点的Map
        //public Dictionary<Token, List<Token>> GetJoinTokenMap()
        //{
        //    return _maptoken;
        //}

        ////取得当前任务实例
        //public TaskInstance GetTaskInstance()
        //{
        //    return _current;
        //}

        //////取得当前流程实例
        ////public ProcessInstance GetProcessInstance()
        ////{
        ////    return _current.GetProcessInstance();
        ////}

        ////取得父级Token
        //public Token GetParent()
        //{
        //    return _parent;
        //}

        ////取得子Token
        //public List<Token> GetChild()
        //{
        //    return _child;
        //}

        ////取得活动的子Token
        //public List<Token> GetActivityChild()
        //{
        //    List<Token> ActivityChild = new List<Token>();

        //    foreach (var item in this.GetChild())
        //    {
        //        if (!item.HasEnded())
        //            ActivityChild.Add(item);
        //    }

        //    return ActivityChild;
        //}

        ////是否有分支或子流程
        //public bool HasChild()
        //{
        //    return (_child.Count == 0) ? false : true;
        //}

        ////是否有激活分支或激活子流程
        //public bool HasActivityChild()
        //{
        //    var childList = this.GetChild().Select(x => x.HasEnded());

        //    return (childList.Contains(false)) ? true : false;
        //}

        ////当前Token是否结束
        //public bool HasEnded()
        //{
        //    return (_tokenState == TokenState.Ended) ? true : false;
        //}

        ////取得TokenState
        //public TokenState GetTokenState()
        //{
        //    return _tokenState;
        //}
        //#endregion

        //#endregion
        #endregion
    }
}
