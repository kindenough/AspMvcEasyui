using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Reflection;
using Zephyr.Data;
using System.Xml;

namespace Zephyr.WorkFlow
{
    public partial class BpmContext
    {
        #region 新版本
        //构造函数
        public BpmContext(BpmConfiguration bpmConfiguration)
        {
            _bpmConfiguration = bpmConfiguration;
            _db = _bpmConfiguration.CreateDbContext();
        }

        //根据指定流程定义XML文件，创建流程定义实例
        public ProcessDefinition createProcessDefinition(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return new ProcessDefinition(this,doc.InnerXml);
        }

        //从数据库存中取得
        public ProcessDefinition loadProcessDefinition(int processDefinitionId)
        {
            var dataProcessDefinition = loadDataProcessDefinition(processDefinitionId);
            return new ProcessDefinition(this,dataProcessDefinition);
        }

        public ProcessDefinition loadProcessDefinition(wf_processDefinition dataProcessDefinition)
        {
            return new ProcessDefinition(this, dataProcessDefinition);
        }

        //发布流程
        public ProcessDefinition deployProcessDefinition(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return new ProcessDefinition(this, doc.InnerXml);
        }

        //创建流程实例
        public ProcessInstance newProcessInstance(string processDefinitionName)
        {
            var dataProcessDefinition = loadDataProcessDefinition(processDefinitionName);
            var processDefinition = loadProcessDefinition(dataProcessDefinition);
            return processDefinition.createProcessInstance();
        }

        public ProcessInstance loadProcessInstance(int processInstanceId)
        {
            var dataProcessInstance = loadDataProcessInstance(processInstanceId);
            var dataProcessDefinition = loadDataProcessDefinition(dataProcessInstance.ProcessDefinitionId);
            var processDefinition = loadProcessDefinition(dataProcessDefinition); 
            return processDefinition.loadProcessInstance(dataProcessInstance);
        }

        public TaskInstance loadTaskInstance(int taskInstanceId) 
        {
            var dataTaskInstance = loadDataTaskInstance(taskInstanceId);
            var processInstance = loadProcessInstance(dataTaskInstance.ProcessInstanceId);
            var task = processInstance.getProcessDefinition().getTaskByName(dataTaskInstance.Task);
            return new TaskInstance(processInstance, task, dataTaskInstance);
        }

        public Token loadToken(int tokenId)
        {
            var dataToken = loadDataToken(tokenId);
            var taskInstance = loadTaskInstance(dataToken.TaskInstanceId);
            return taskInstance.loadToken(dataToken);
        }
 
        //保存流程实例
        public int save(ProcessInstance processInstance)
        {
            var model = processInstance.getProcessInstanceModel();
            var result = save<wf_processInstance>(model);
            return result;
        }

        //保存工作项实例
        public int save(TaskInstance taskInstance) 
        {
            var model = taskInstance.getTaskInstanceModel();
            var result = save<wf_taskInstance>(model);
            return result;
        }

        public int save(Token token) 
        {
            var model = token.getTokenModel();
            var result = save<wf_token>(model);
            return result;
        }

        public int save(TransitionInstance transitionInstance)
        {
            var model = transitionInstance.getTransitionInstanceModel();
            var result = save<wf_transitionInstance>(model);
            return result;
        }

        public int save(ProcessDefinition processDefinition)
        {
            var model = processDefinition.getProcessDefinitionModel();
            var result = save<wf_processDefinition>(model);
            return result;
        }


        public IList getTaskList() { return null; }
        public IList getTaskList(string actorId) { return null; }
        public IList getGroupTaskList(IList actorIds) { return null; }
        public TaskInstance loadTaskInstance(string taskInstanceId) { return null; }
        //public TaskInstance loadTaskInstanceForUpdate(string taskInstanceId) { return null; }
        public Token loadToken(string tokenId) { return null; }
        //public Token loadTokenForUpdate(string tokenId) { return null; }

        //public ProcessInstance loadProcessInstanceForUpdate(string processInstanceId) { return null; }

        //public void save(ProcessInstance processInstance) { }
    
        //public void save(TaskInstance taskInstance) { }
        public void setRollbackOnly() { }
        #endregion

        #region 河联版本
        #region 私有
        //#region 私有属性
        //private DataAccess _da;
        //private CurrentUserInfo _currentuserinfo;
        //#endregion
        #endregion

        #region 属性
        //public DataAccess da
        //{
        //    get { return _da; }
        //}

        //public CurrentUserInfo CurrentUser
        //{
        //    get { return _currentuserinfo; }
        //}

        //public HOLDMsg Msg = new HOLDMsg();
        #endregion

        //#region 构造函数
        //public BpmContext()
        //{
        //    //_da = new DataAccess(true);
        //    //_currentuserinfo = new CurrentUserInfo();
        //    //_currentuserinfo.IDUser = GetUserID();
        //    //_currentuserinfo.UserName = GetUserName();
        //    //_currentuserinfo.IDRole = GetUserRole();
        //    //_currentuserinfo.IDOrganize = GetUserOrg();
        //}
        //#endregion

//        #region 方法
//        #region 取得自定义ID
//        public string GetCustomID(string numberType)
//        {
//            int intReturn = 0;
//            string sMsg = "";
//            string sSql = "";
//            DataTable dt = new DataTable();

//            sSql = string.Format(@"
//update wf_CustomID 
//set
//current_no=Temp.current_no+1,
//UpdatePerson='{1}',
//UpdateDate='{2}'
//from wf_CustomID as Temp
//where Temp.Type='{0}'
//            ", numberType, _currentuserinfo.UserName, DateTime.Now);

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);

//            if (sMsg.Length == 0)
//            {
//                if (intReturn > 0)
//                {
//                    MsgDeal("更新自定义主键成功！", MsgType.Success);
//                }
//            }
//            else
//            {
//                MsgDeal("更新自定义主键失败：" + sMsg, MsgType.Error);
//                return "";
//            }

//            sSql = string.Format(@"
//select * 
//from wf_CustomID
//where Type='{0}'
//                ", numberType);

//            sMsg = _da.execQuery(sSql, ref dt);
//            if (sMsg.Length == 0 && dt.Rows.Count > 0)
//            {
//                MsgDeal("获取自定义主键成功", MsgType.Success);
//                return HOLDConvert.ToString(dt.Rows[0]["current_no"]);
//            }
//            else
//            {
//                MsgDeal("获取自定义主键失败:" + sMsg, MsgType.Error);
//                return "";
//            }

//        }
//        #endregion

//        #region BpmConfiguration
//        private BpmConfiguration _bpmConfig;

//        public BpmConfiguration bpmConfig { set { _bpmConfig = value; } }

//        public BpmConfiguration getJbpmConfiguration()
//        {
//            return _bpmConfig;
//        }

//        #endregion

//        #region 流程定义
//        //发布工作流
//        public void deployProcessDefinition(ProcessDefinition processDefinition)
//        {
//            //humin
//            //新建一个流程发布管理界面
//            //在wf_ProcessDefinition中新增一条记录，版本号根据名称取最大值+1，将XML的数据存到数据库中
//        }

//        //根据流程定义名称取得最新流程定义XML
//        public ProcessDefinition findLatestProcessDefinition(string ProcessDefName)
//        {
//            string xmlstring = "";

//            //根据名称从数据库里取得XMLData
//            DataTable dt = new DataTable();
//            string sSql = string.Format(@"
//select top 1 * 
//from wf_ProcessDef
//where ProcessDefName='{0}'
//order by VersionNo desc      
//            ", ProcessDefName);

//            string sMsg = _da.execQuery(sSql, ref dt);
//            if (sMsg.Length == 0 && dt.Rows.Count > 0)
//            {
//                xmlstring = dt.Rows[0]["ProcessDefXMLPath"].ToString();
//                ProcessDefinition processDefinition = ProcessDefinition.LoadProcessDefinition(xmlstring);

//                MsgDeal("获取最新流程定义对象成功！", MsgType.Success);
//                return processDefinition;
//            }
//            else
//            {
//                MsgDeal("获取最新流程定义对象失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//        }

//        //根据流程定义的ID取得流程定义
//        public ProcessDefinition LoadProcessDefinitionById(string processDefinitionById)
//        {
//            string xmlstring = "";

//            //根据名称从数据库里取得XMLData
//            DataTable dt = new DataTable();
//            string sSql = string.Format(@"
//select  * 
//from wf_ProcessDef
//where IDProcessDef='{0}'     
//            ", processDefinitionById);

//            string sMsg = _da.execQuery(sSql, ref dt);
//            if (sMsg.Length == 0 && dt.Rows.Count > 0)
//            {
//                xmlstring = dt.Rows[0]["ProcessDefXMLPath"].ToString();
//                ProcessDefinition processDefinition = ProcessDefinition.LoadProcessDefinition(xmlstring);

//                MsgDeal("获取流程定义对象成功！", MsgType.Success);
//                return processDefinition;
//            }
//            else
//            {
//                MsgDeal("获取流程定义对象失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//        }
//        #endregion

//        #region 流程实例
//        //根据流程实例主键加载流程实例
//        public ProcessInstance loadProcessInstance(string processInstanceId)
//        {
//            ProcessInstance processInst = null;
//            string tokenid = "";

//            //加载流程实例   load ProcessInstance
//            ProcessDefinition processDefinition = null;
//            string id = "";
//            string name = "";
//            ProcessState state = ProcessState.Stop;
//            Hashtable variable = new Hashtable();
//            DateTime? startTime = null;
//            DateTime? endTime = null;
//            BpmContext bpmContext = this;
//            TokenState tokenState = TokenState.Notactive;
//            string idtaskinst = "";
//            List<Token> RootTempToken = new List<Token>();
//            ProcessInstance parentProcessInst = null;

//            //获取到流程实例的信息
//            DataTable dt = new DataTable();

//            string sSql = string.Format(@"
//select  pi.*,pd.ProcessDefXMLPath,taskInst.IDTaskInst,token.TokenState
//        ,taskInst.IDTask,wf_Variable.Value as bizKey
//from wf_ProcessInst as pi
//left join wf_ProcessDef as pd on pd.IDProcessDef = pi.IDProcessDef 
//left join wf_Token as token on  pi.IDRootToken=token.IDToken
//left join wf_TaskInst as taskInst on taskInst.IDTaskInst=token.IDTaskInst   
//left join wf_Variable on wf_Variable.Name = pi.BizKeyName and InstType='{1}' and InstTypeValue='{0}'
//where pi.IDProcessInst='{0}'
//                ", processInstanceId, InstanceType.ProcessInstance);

//            string sMsg = _da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载流程实例失败：" + sMsg, MsgType.Success);
//                return null;
//            }
//            else
//            {
//                if (dt.Rows.Count > 0)
//                {
//                    processDefinition = ProcessDefinition.LoadProcessDefinition(dt.Rows[0]["ProcessDefXMLPath"].ToString());
//                    id = HOLDConvert.ToString(dt.Rows[0]["IDProcessInst"]);
//                    name = processDefinition.GetName();
//                    tokenid = HOLDConvert.ToString(dt.Rows[0]["IDRootToken"]);           //加载Token
//                    state = EunmConvert.ToProcessState(dt.Rows[0]["ProcessState"]);
//                    variable = new Hashtable();
//                    startTime = HOLDConvert.ToDateTime(dt.Rows[0]["StartTime"]);
//                    endTime = HOLDConvert.ToDateTime(dt.Rows[0]["EndTime"]);
//                    idtaskinst = HOLDConvert.ToString(dt.Rows[0]["IDTaskInst"]);
//                    tokenState = EunmConvert.ToTokenState(dt.Rows[0]["TokenState"]);

//                    #region 取得流程变量的数据
//                    dt = new DataTable();
//                    sSql = string.Format(@"
//select *
//from wf_Variable
//where InstType='{0}'
//and InstTypeValue='{1}'
//                    ", InstanceType.ProcessInstance, processInstanceId);

//                    sMsg = da.execQuery(sSql, ref dt);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("加载流程实例的变量失败：" + sMsg, MsgType.Error);
//                    }
//                    else
//                    {
//                        IDictionary<string, string> ProcessVars = processDefinition.GetProcessVars();
//                        Dictionary<string, object> VarsValue = new Dictionary<string, object>();

//                        foreach (DataRow dr in dt.Select())
//                        {
//                            VarsValue.Add(HOLDConvert.ToString(dr["Name"]), dr["Value"]);
//                        }
//                        foreach (var i in ProcessVars)
//                        {
//                            variable[i.Key] = (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : null;
//                        }

//                        MsgDeal("加载流程实例的变量成功！：", MsgType.Success);
//                    }

//                    #endregion

//                    #region 取得父流程的数据
//                    dt = new DataTable();
//                    sSql = string.Format(@"
//select *
//from wf_SubProcessInst
//where CurrentIDProcessInst='{0}'
//                    ", processInstanceId);

//                    sMsg = da.execQuery(sSql, ref dt);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("加载父流程失败：" + sMsg, MsgType.Error);
//                    }
//                    else
//                    {
//                        if (dt.Rows.Count > 0)
//                        {
//                            parentProcessInst = loadProcessInstance(HOLDConvert.ToString(dt.Rows[0]["ParentIDProcessInst"]));
//                        }

//                        MsgDeal("加载父流程成功！：", MsgType.Success);
//                    }

//                    #endregion
//                }
//            }

//            processInst = new ProcessInstance(processDefinition, id, name, tokenid, state, variable, startTime, endTime, bpmContext, idtaskinst, tokenState, parentProcessInst);

//            MsgDeal("加载流程实例成功！", MsgType.Success);
//            return processInst;
//        }

//        //新增一个流程实例化数据
//        public ProcessInstance newProcessInstance(string processDefName)
//        {
//            return newProcessInstance(processDefName, "");
//        }

//        public ProcessInstance newProcessInstance(string processDefName, string IDProcessInst)
//        {
//            ProcessDefinition processDef = findLatestProcessDefinition(processDefName);
//            BpmContext bpmContext = this;
//            ProcessDefinition processDefinition = processDef;
//            string id = GetCustomID(InstanceType.ProcessInstance.ToString());
//            string name = processDef.GetName();
//            Token token = null;
//            ProcessState state = ProcessState.Stop;
//            Hashtable variable = new Hashtable();
//            DateTime? startTime = null;
//            DateTime? endTime = null;
//            ProcessInstance parentProcessInst = (IDProcessInst.Length == 0) ? null : this.loadProcessInstance(IDProcessInst);

//            #region 赋变量
//            IDictionary<string, string> ProcessVars = processDefinition.GetProcessVars();
//            object o = HOLDSession.Get("SesBPMVariables");
//            Dictionary<string, object> VarsValue = (o == null) ? (new Dictionary<string, object>()) : (Dictionary<string, object>)o;
//            foreach (var i in ProcessVars)
//            {
//                variable[i.Key] = (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : null;
//            }

//            MsgDeal("创建流程实例的流程变量成功！", MsgType.Success);
//            #endregion

//            ProcessInstance processInst = new ProcessInstance(processDefinition, id, name, token, state, variable, startTime, endTime, bpmContext, parentProcessInst);

//            //流程实例ID记在Session主要用于工程信息中冗余保存一份
//            HOLDSession.Add("SesIDProcessInst", processInst.GetId());

//            MsgDeal("创建流程实例对象成功！", MsgType.Success);
//            return processInst;
//        }

//        #endregion

//        #region 待办任务
//        //加载待办任务
//        public IList<TaskInstance> getTaskList(ProcessInstance pi)
//        {
//            IList<TaskInstance> taskList = new List<TaskInstance>();
//            IList<Token> todoToken = pi.GetToDoToken();

//            foreach (var item in todoToken)
//            {
//                taskList.Add(item.GetTaskInstance());
//            }

//            return taskList;
//        }

//        //根据参与人取得待办工作项列表
//        public IList<Task> getTaskList(String actorId)
//        {
//            return null;
//        }

//        //待办任务列表
//        public IList getGroupTaskList(IList actorIds)
//        {
//            return null;
//        }

//        //根据当前工作项实例ID取得是否当前工作项
//        public bool IsCurrentTaskInst(string IDTaskInst)
//        {
//            if (IDTaskInst.Length == 0) return false;

//            string IDUser = HOLDConvert.ToString(_currentuserinfo.IDUser);
//            string IDRole = HOLDConvert.ToString(_currentuserinfo.IDRole);
//            string IDOrganize = HOLDConvert.ToString(_currentuserinfo.IDOrganize);

//            string sMsg = "";
//            string sSql = "";
//            DataTable dt = new DataTable();
//            bool IsCurrent = false;
//            sSql = string.Format(@"
//select wf_ProcessDef.ProcessDefXMLPath,wf_ProcessDef.IDProcessDef,wf_TaskInst.IDTask,wf_ProcessInst.IDProcessInst
//    ,wf_Variable.Value as BizKeyValue,wf_ProcessInst.BizKeyName
//from wf_ProcessInst
//left join wf_TaskInst on wf_ProcessInst.IDProcessInst=wf_TaskInst.IDProcessInst
//left join wf_ProcessDef on wf_ProcessInst.IDProcessDef=wf_ProcessDef.IDProcessDef
//left join wf_Variable on wf_ProcessInst.BizKeyName = wf_Variable.name and wf_Variable.InstType='ProcessInstance' and wf_ProcessInst.IDProcessInst = wf_Variable.InstTypeValue
//where wf_ProcessInst.ProcessState='Run' and wf_TaskInst.TaskState='Run'
//and wf_TaskInst.IDTaskInst in 
//(
//    select wf_Assign.IDTaskInst
//    from wf_Assign 
//    left join wf_TaskInst on wf_TaskInst.IDTaskInst=wf_Assign.IDTaskInst
//    left join wf_ProcessInst on wf_ProcessInst.IDProcessInst=wf_TaskInst.IDProcessInst
//    left join wf_Entrust on wf_Assign.value=wf_Entrust.Transfer and wf_Assign.type='User' and wf_Assign.IDTaskInst=wf_Entrust.IDTaskInst
//    where wf_ProcessInst.ProcessState='Run' 
//    and wf_TaskInst.TaskState='Run' and wf_TaskInst.IDTaskInst='{3}'
//    and ((wf_Assign.Type='User' and (CASE WHEN isnull(wf_Entrust.Entruster,'')='' THEN wf_Assign.value ELSE wf_Entrust.Entruster END)='{0}')
//    or (wf_Assign.Type='Role' and wf_Assign.value='{1}')
//    or (wf_Assign.Type='Org' and wf_Assign.value='{2}'))
//    group by wf_Assign.IDTaskInst
//)
//            ", IDUser, IDRole, IDOrganize, IDTaskInst);

//            sMsg = _da.execQuery(sSql, ref dt);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("判断是否已发送失败：" + sMsg, MsgType.Error);
//                return IsCurrent;
//            }

//            if(dt.Rows.Count>0)
//            {
//                IsCurrent = true;
//            }
//            MsgDeal("判断是否已发送成功！", MsgType.Success);
           

//            return IsCurrent;
//        }

//        #endregion

//        #region 委托代理


//        #endregion

//        #region 工作项实例
//        //根据工作项实例ID直接加载
//        public TaskInstance loadTaskInstance(string taskInstanceId)
//        {
//            return loadTaskInstance(taskInstanceId, null);
//        }

//        //根据工作项实例ID和流程实例对象加载
//        public TaskInstance loadTaskInstance(string taskInstanceId, ProcessInstance pi)
//        {
//            TaskInstance taskInstance = null;
//            ProcessInstance processInstance = pi;
//            Task task = null;
//            string id = taskInstanceId;
//            DateTime? createTime = null;
//            DateTime? startTime = null;
//            DateTime? endTime = null;
//            TaskState taskState = TaskState.Stop;
//            int actorID = 0;
//            Dictionary<int, string[]> pooledActors = new Dictionary<int, string[]>();
//            Hashtable variable = new Hashtable();
//            string arrivedTransInstancesId = "";

//            #region 加载工作项实例
//            DataTable dt = new DataTable();
//            string sMsg = "";
//            string sSql = "";

//            sSql = string.Format(@"
//select * 
//from wf_TaskInst
//where IDTaskInst='{0}'
//                ", taskInstanceId);

//            sMsg = _da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载工作项实例对象失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//            else
//            {
//                if (dt.Rows.Count > 0)
//                {
//                    if (processInstance == null)
//                        processInstance = loadProcessInstance(HOLDConvert.ToString(dt.Rows[0]["IDProcessInst"]));

//                    createTime = HOLDConvert.ToDateTime(dt.Rows[0]["CreateDate"]);
//                    startTime = HOLDConvert.ToDateTime(dt.Rows[0]["StartTime"]);
//                    endTime = HOLDConvert.ToDateTime(dt.Rows[0]["EndTime"]);
//                    taskState = EunmConvert.ToTaskState(dt.Rows[0]["TaskState"]);
//                    actorID = HOLDConvert.ToInt(dt.Rows[0]["ActorID"], 0);
//                    task = processInstance.GetProcessDefinition().GetTaskByName(dt.Rows[0]["IDTask"].ToString());
//                    pooledActors = new Dictionary<int, string[]>();
//                    variable = new Hashtable();
//                    arrivedTransInstancesId = HOLDConvert.ToString(dt.Rows[0]["ArrivedTransIDs"]); ;

//                    #region 取得流程变量的数据
//                    sSql = string.Format(@"
//select *
//from wf_Variable
//where InstType='{0}'
//and InstTypeValue='{1}'
//                    ", InstanceType.TaskInstance, taskInstance);

//                    dt = new DataTable();
//                    sMsg = da.execQuery(sSql, ref dt);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("获取工作项实例的变量失败：" + sMsg, MsgType.Error);
//                        return null;
//                    }
//                    else
//                    {
//                        IDictionary<string, string> TaskVars = task.TaskVars;
//                        Dictionary<string, object> VarsValue = new Dictionary<string, object>();

//                        foreach (DataRow dr in dt.Select())
//                        {
//                            VarsValue.Add(HOLDConvert.ToString(dr["Name"]), dr["Value"]);
//                        }

//                        foreach (var i in TaskVars)
//                        {
//                            variable[i.Key] = (VarsValue.ContainsKey(i.Key)) ? VarsValue[i.Key] : null;
//                        }

//                        MsgDeal("获取工作项实例的变量成功！", MsgType.Success);
//                    }

//                    #endregion

//                    #region 加载任务参与者
//                    dt = new DataTable();
//                    sSql = string.Format(@"
//select *
//from wf_AssignDef
//where IDTask='{0}'
//and IDProcessDef='{1}'
//                ", task.Name, processInstance.GetProcessDefinition().GetId());

//                    sMsg = da.execQuery(sSql, ref dt);
//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("加载工作项实例参与者失败：" + sMsg, MsgType.Error);
//                        return null;
//                    }

//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        pooledActors.Add(i, new string[] { HOLDConvert.ToString(dt.Rows[i]["InstanceType"]), HOLDConvert.ToString(dt.Rows[i]["Value"]) });
//                    }

//                    MsgDeal("加载工作项实例参与者成功！", MsgType.Success);
//                    #endregion
//                }
//            }
//            #endregion

//            taskInstance = new TaskInstance(processInstance, task, id, createTime, startTime, endTime, taskState, actorID, pooledActors, variable, arrivedTransInstancesId);

//            MsgDeal("加载工作项实例对象成功！", MsgType.Success);
//            return taskInstance;
//        }

//        #endregion

//        #region 令牌
//        #region 加载
//        //根据令牌ID直接加载
//        public Token loadToken(string tokenId)
//        {
//            return loadToken(tokenId, null);
//        }

//        //根据令牌ID和流程实例对象直接加载
//        public Token loadToken(string tokenId, ProcessInstance pi)
//        {
//            Token token = null;
//            List<Token> child = new List<Token>();
//            string id = tokenId;

//            #region 取得Token的数据
//            string sSql = "";
//            string sMsg = "";
//            DataTable dt = new DataTable();

//            sSql = string.Format(@"
//select * 
//from wf_Token
//where IDToken='{0}'
//                ", tokenId);

//            sMsg = da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载令牌失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//            else
//            {
//                if (dt.Rows.Count > 0)
//                {
//                    if (pi == null)
//                        pi = loadProcessInstance(HOLDConvert.ToString(dt.Rows[0]["IDProcessInst"]));

//                    Token RootToken = pi.GetRootToken();

//                    if (RootToken.GetId() == tokenId)
//                        token = RootToken;
//                    else
//                    {
//                        token = GetToken(RootToken, tokenId);

//                        //在临时TempToken里去寻找
//                        if (token == null)
//                        {
//                            if (pi.GetTempToken() != null)
//                            {
//                                foreach (var item in pi.GetTempToken())
//                                {
//                                    if (item.GetId() == tokenId)
//                                        token = item;
//                                    else
//                                        token = GetToken(item, tokenId);

//                                    if (token != null)
//                                        break;
//                                }
//                            }
//                            else
//                            {   //Todo for fix bug load tempToken 
//                                //token = new Token(taskInst, null, new List<Token>(), idRootToken, rootTokenState);
//                                //throw new Exception("临时token未生成！加载loadtoken错误！");

//                                //如果临时token未生成
//                                //TaskInstance current = loadTaskInstance(HOLDConvert.ToString(dt.Rows[0]["IDTaskInst"]), pi);
//                                //Token parent = null;
//                                //List<Token> childTokens = new List<Token>();
//                                //string tid = HOLDConvert.ToString(dt.Rows[0]["TempIDToken"]);
//                                //TokenState tokenState = EunmConvert.ToTokenState(dt.Rows[0]["TokenState"]);

//                                //token = new Token(current, parent, childTokens, tid, tokenState);
//                            }
//                        }
//                    }
//                }
//            }
//            #endregion

//            MsgDeal("加载令牌成功！", MsgType.Success);
//            return token;
//        }

//        //加载流程的临时JoinToken
//        public List<Token> loadTempToken(ProcessInstance processInstance)
//        {
//            List<Token> TempToken = new List<Token>();

//            #region 取得Token的数据
//            string sSql = "";
//            string sMsg = "";
//            DataTable dt = new DataTable();

//            sSql = string.Format(@"
//select wf_TempToken.*,wf_Token.IDTaskInst,wf_Token.TokenState 
//from wf_TempToken
//left join wf_Token on wf_Token.IDToken=wf_TempToken.TempIDToken
//where wf_TempToken.IDProcessInst='{0}'
//                ", processInstance.GetId());

//            sMsg = da.execQuery(sSql, ref dt);

//            #endregion

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载流程实例的临时令牌失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//            else
//            {
//                foreach (DataRow dr in dt.Select(""))
//                {
//                    TaskInstance current = loadTaskInstance(HOLDConvert.ToString(dr["IDTaskInst"]), processInstance);
//                    Token parent = null;
//                    List<Token> child = new List<Token>();
//                    string id = HOLDConvert.ToString(dr["TempIDToken"]);
//                    TokenState tokenState = EunmConvert.ToTokenState(dr["TokenState"]);

//                    Token token = new Token(current, parent, child, id, tokenState);

//                    //递归加载临时节点的子Token
//                    TempToken.Add(token);
//                }
//            }

//            MsgDeal("加载流程实例的临时令牌成功！", MsgType.Success);
//            return TempToken;
//        }

//        //加载节点的MapToken
//        public Dictionary<Token, List<Token>> loadMapToken(string currentIDToken, ProcessInstance processInstance)
//        {
//            Dictionary<Token, List<Token>> MapToken = new Dictionary<Token, List<Token>>();

//            #region 取得Token的数据
//            string sSql = "";
//            string sMsg = "";
//            DataTable dt = new DataTable();

//            sSql = string.Format(@"
//select MapToken.*
//    , ParentToken.IDTaskInst as ParentIDTaskInst
//    ,ArriveToken.IDTaskInst as ArriveIDTaskInst
//    ,ParentToken.TokenState as ParentTokenState
//    ,ArriveToken.TokenState as ArriveTokenState
//from wf_MapToken as MapToken
//left join wf_Token as ArriveToken on ArriveToken.IDToken=MapToken.ArriveIDToken
//left join wf_Token as ParentToken on ParentToken.IDToken=MapToken.ParentIDToken
//where MapToken.CurrentIDToken='{0}'
//                ", currentIDToken);

//            sMsg = da.execQuery(sSql, ref dt);

//            #endregion

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载MapToken失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//            else
//            {
//                HOLDDataSet hds = new HOLDDataSet();
//                DataTable dtParent = hds.SelectGroupByInto("parent", dt, "ParentIDToken", "", "ParentIDToken");

//                foreach (DataRow drParent in dtParent.Select(""))
//                {
//                    Token parentToken = null;
//                    List<Token> arriveTokens = new List<Token>();

//                    parentToken = loadToken(HOLDConvert.ToString(drParent["ParentIDToken"]), processInstance);

//                    foreach (DataRow dr in dt.Select(string.Format("ParentIDToken='{0}'", drParent["ParentIDToken"])))
//                    {
//                        Token arriveToken = null;
//                        arriveToken = loadToken(HOLDConvert.ToString(dr["ArriveIDToken"]), processInstance);

//                        if (arriveToken != null)//add by liuhuisheng for avoid null value
//                            arriveTokens.Add(arriveToken);
//                    }

//                    try
//                    {
//                        //递归加载临时节点的子Token
//                        if (parentToken != null)//add by liuhuisheng for avoid null value
//                        {
//                            if (!MapToken.ContainsKey(parentToken))
//                                MapToken.Add(parentToken, new List<Token>());
//                        }
//                    }
//                    catch (Exception ex)
//                    { throw ex; }

//                    if (arriveTokens.Count > 0)//add by liuhuisheng for avoid null value
//                         MapToken[parentToken].AddRange(arriveTokens);
//                }
//            }

//            MsgDeal("加载MapToken成功！", MsgType.Success);
//            return MapToken;
//        }
//        #endregion

//        #region 递归
//        //从父子关系里去寻找指定的令牌
//        private Token GetToken(Token parentToken, string currentId)
//        {
//            Token currentToken = null;
//            foreach (Token i in parentToken.GetChild())
//            {
//                if (i.GetId() == currentId)
//                {
//                    currentToken = i;
//                    break;
//                }
//                currentToken = GetToken(i, currentId);
//                if (currentToken != null) break;
//            }

//            return currentToken;
//        }

//        //递归循环，加载所有的子Token
//        public void loopToken(Token parentToken, List<Token> parentTokenChilds)
//        {
//            string pid = parentToken.GetId();

//            string sSql = "";
//            string sMsg = "";
//            DataTable dt = new DataTable();
//            DataTable dtRootTemp = new DataTable();
//            DataTable dtMap = new DataTable();

//            #region 加载parentTokenChilds
//            sSql = string.Format(@"
//select * 
//from wf_Token
//where ParentIDToken ='{0}'
//                ", pid);

//            sMsg = da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("递归加载子令牌失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            if (dt.Rows.Count > 0)
//            {
//                foreach (DataRow dr in dt.Select())
//                {
//                    TaskInstance current = loadTaskInstance(HOLDConvert.ToString(dr["IDTaskInst"]), parentToken.GetTaskInstance().GetProcessInstance());
//                    Token parent = parentToken;
//                    List<Token> child = new List<Token>();
//                    string id = HOLDConvert.ToString(dr["IDToken"]);
//                    TokenState tokenState = EunmConvert.ToTokenState(dr["TokenState"]);

//                    Token token = new Token(current, parent, child, id, tokenState);
//                    parentTokenChilds.Add(token);
//                }
//            }
//            #endregion
//        }

//        #endregion

//        #endregion

//        #region 弧线实例
//        //根据弧线实例ID直接加载
//        public TransitionInstance loadTransitionInstance(string transitionInstanceId)
//        {
//            return loadTransitionInstance(transitionInstanceId, null);
//        }

//        //根据弧线实例ID和流程实例对象加载
//        public TransitionInstance loadTransitionInstance(string transitionInstanceId,ProcessInstance processInstance)
//        {
//            TransitionInstance transitionInstance = null;

//            string id = transitionInstanceId;
//            TaskInstance fromTaskInst = null;
//            bool isExecuted = false;
//            Transition transition = null;

//            #region 加载弧线实例
//            DataTable dt = new DataTable();
//            string sMsg = "";
//            string sSql = "";

//            sSql = string.Format(@"
//select * 
//from wf_TransitionInst
//where IDTransitionInst='{0}'
//                ", transitionInstanceId);

//            sMsg = _da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("加载弧线实例对象失败：" + sMsg, MsgType.Error);
//                return null;
//            }
//            else
//            {
//                if (dt.Rows.Count > 0)
//                {
//                    id = HOLDConvert.ToString(dt.Rows[0]["IDTransitionInst"]);
//                    isExecuted = HOLDConvert.ToBool(dt.Rows[0]["IsExcuted"]);
//                    string IDTaskInst = HOLDConvert.ToString(dt.Rows[0]["IDTaskInst"]);
//                    string IDTransition = HOLDConvert.ToString(dt.Rows[0]["IDTransition"]);

//                    fromTaskInst = loadTaskInstance(IDTaskInst, processInstance);
//                    foreach (Transition item in fromTaskInst.GetTask().LeavingTransitions)
//                    {
//                        if (item.GetId() == IDTransition)
//                        {
//                            transition = item;
//                            break;
//                        }
//                    }
//                }
//            }
//            #endregion

//            transitionInstance = new TransitionInstance(id, fromTaskInst, isExecuted, transition);

//            MsgDeal("加载弧线实例对象成功！", MsgType.Success);
//            return transitionInstance;
//        }
//        #endregion

//        #region 保存
//        #region 保存流程实例数据
//        public void save(ProcessInstance processInstance)
//        {
//            int intReturn = 0;
//            string sMsg = "";
//            string sSql = "";
//            DataTable dt = new DataTable();
//            string userName = _currentuserinfo.UserName;

//            ProcessDefinition pd = processInstance.GetProcessDefinition();

//            #region 保存流程实例表
//            #region 更新
//            sSql = string.Format(@" 
//update  wf_ProcessInst
//set
//    IDProcessDef = '{1}',ProcessInstName='{2}',VersionNo ='{3}',IDRootToken = '{4}',BizKeyName='{5}',
//    StartTime = '{6}',EndTime = '{7}',ProcessState = '{8}', UpdatePerson = '{9}',UpdateDate = '{10}'
//where   IDProcessInst = '{0}'"
//                , processInstance.GetId(), pd.GetId(), pd.GetName(), pd.GetVersion(), processInstance.GetRootToken().GetId(), pd.GetBizKeyName(),
//                processInstance.GetStartTime(), processInstance.GetEndTime(), processInstance.GetState(), userName, DateTime.Now);

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("更新流程实例表失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            MsgDeal("更新流程实例表成功！", MsgType.Success);
//            #endregion

//            #region 插入
//            if (intReturn == 0)
//            {
//                sSql = string.Format(@"
//insert into wf_ProcessInst
//(
//    IDProcessInst,IDProcessDef,ProcessInstName,VersionNo,IDRootToken,BizKeyName,
//    StartTime,EndTime,ProcessState,CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}',
//    '{6}','{7}','{8}','{9}','{10}','{9}','{10}'
//)
//                ", processInstance.GetId(), pd.GetId(), pd.GetName(), pd.GetVersion(), processInstance.GetRootToken().GetId(), pd.GetBizKeyName(),
//                     processInstance.GetStartTime(), processInstance.GetEndTime(), processInstance.GetState(), userName, DateTime.Now
//                     );

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("插入流程实例表失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("插入流程实例表成功！", MsgType.Success);
//            }
//            #endregion
//            #endregion

//            #region 保存父子流程实例关系表
//            if (processInstance.GetParentProcessInst() != null)
//            {
//                #region 更新
//                string parentProcessInstID = processInstance.GetParentProcessInst().GetId();
//                sSql = string.Format(@" 
//update  wf_SubProcessInst
//set
//    ParentIDProcessInst='{1}', UpdatePerson = '{2}',UpdateDate = '{3}'
//where   CurrentIDProcessInst = '{0}'"
//                    , processInstance.GetId(), parentProcessInstID, userName, DateTime.Now);

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("更新父子流程实例关系表失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("更新父子流程实例关系表成功！", MsgType.Success);
//                #endregion

//                #region 插入
//                if (intReturn == 0)
//                {
//                    sSql = string.Format(@"
//insert into wf_SubProcessInst
//(
//    CurrentIDProcessInst,ParentIDProcessInst
//    ,CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{2}','{3}'
//)
//                ", processInstance.GetId(), parentProcessInstID, userName, DateTime.Now
//                         );

//                    sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("插入父子流程实例关系表失败：" + sMsg, MsgType.Error);
//                        return;
//                    }

//                    MsgDeal("插入父子流程实例关系表成功！", MsgType.Success);
//                }
//                #endregion
//            }
//            #endregion

//            #region 保存流程实例变量
//            IDictionary<string, string> processVars = pd.GetProcessVars();
//            foreach (var variable in processVars)
//            {
//                #region 更新
//                object o = processInstance.GetVariable(variable.Key);
//                sSql = string.Format(@"
//update wf_Variable 
//set 
//    ValueType='{0}',Value='{1}',UpdatePerson='{2}',UpdateDate='{3}'
//where InstType='{4}'
//and InstTypeValue='{5}'
//and Name='{6}'
//                    ", variable.Value, o, userName, DateTime.Now, InstanceType.ProcessInstance, processInstance.GetId(), variable.Key);

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("更新流程实例表的变量失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("更新流程实例表的变量成功！", MsgType.Success);
//                #endregion

//                #region 插入
//                if (intReturn == 0)
//                {
//                    sSql = string.Format(@"
//insert into wf_Variable
//(
//    Name,ValueType,Value,InstType,InstTypeValue,CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{5}','{6}'
//)
//                        ", variable.Key, variable.Value, o, InstanceType.ProcessInstance, processInstance.GetId(), userName, DateTime.Now);
//                    sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("插入流程实例表的变量失败：" + sMsg, MsgType.Error);
//                        return;
//                    }

//                    MsgDeal("插入流程实例表的变量成功！", MsgType.Success);
//                }
//                #endregion
//            }

//            MsgDeal("保存流程实例表的变量成功！", MsgType.Success);
//            #endregion

//            MsgDeal("保存流程实例表成功！", MsgType.Success);
//        }

//        //保存临时Token
//        public void saveTempToken(ProcessInstance processInstance, Token TempToken)
//        {
//            string sSql = "";
//            string sMsg = "";
//            int intReturn = 0;
//            string userName = _currentuserinfo.UserName;

//            DataTable dt = new DataTable();

//            sSql = String.Format(@" 
//update  wf_TempToken 
//set     UpdatePerson = '{2}' , UpdateDate = '{3}'
//where   IDProcessInst = '{0}' and TempIDToken='{1}'",
//                     processInstance.GetId(), TempToken.GetId(), userName, DateTime.Now);

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("更新根令牌临时表失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            if (intReturn == 0)
//            {
//                sSql = String.Format(@" 
//insert into  wf_TempToken 
//(
//    IDProcessInst,TempIDToken
//    ,CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{2}','{3}'
//)
//                    ",
//                     processInstance.GetId(), TempToken.GetId(), userName, DateTime.Now);

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("插入根令牌临时表失败：" + sMsg, MsgType.Error);
//                    return;
//                }
//            }

//            MsgDeal("保存根令牌临时表成功！", MsgType.Success);
//        }
//        #endregion

//        #region 保存工作项
//        public void save(TaskInstance taskInstance)
//        {
//            string sSql = ""; 
//            string sMsg = "";
//            int intReturn = 0;
//            ProcessInstance pi = taskInstance.GetProcessInstance();
//            ProcessDefinition pd = pi.GetProcessDefinition();
//            Task task = taskInstance.GetTask();
//            string userName = _currentuserinfo.UserName;

//            DataTable dt = new DataTable();
//            string DueDateTime=null;
//            string ExtendedDateTime = null ;
//            #region 保存工作项

//            //获取到期时间、超期时间
//            DateTime InstanceStartTime=HOLDConvert.ToDateTime( taskInstance.GetInstanceStartTime(),DateTime.Now);
//            string DueAndExtendedDate = GetDueAndExtendedDateTime(task.DueTime, task.ExtendedTime, InstanceStartTime);
//            string [] sDate=new String[4];
//            sDate=DueAndExtendedDate.Split(';');
//            if (sDate.Length > 0)
//            {
//                task.DueTime =HOLDConvert.ToInt( sDate[0],0);
//                task.ExtendedTime =HOLDConvert.ToInt( sDate[1],0);
//                DueDateTime = sDate[2];
//                ExtendedDateTime = sDate[3];
//            }

//            #region 更新
//            sSql = String.Format(@" 
//update  wf_TaskInst 
//set     IDProcessInst ='{1}',IDTask ='{2}',VersionNo='{3}',TaskInstName='{4}',Description='{5}', ActorID ='{6}',
//        StartTime ='{7}',EndTime ='{8}',DueTime ='{9}',TaskState ='{10}',UpdatePerson='{11}',UpdateDate ='{12}',
//        ArrivedTransIDs='{13}',NodeType='{14}',ExtendedTime='{15}',DueDateTime='{16}'  ,ExtendedDateTime='{17}'
//where   IDTaskInst = '{0}'",
//                       taskInstance.GetInstanceId(), pi.GetId(), task.Name, pd.GetVersion(),task.Name, task.Description, taskInstance.GetInstanceActors(),
//                       taskInstance.GetInstanceStartTime(), taskInstance.GetInstanceEndTime(), task.DueTime, taskInstance.GetInstanceStatus(), userName,
//                       DateTime.Now, taskInstance.GetArrivedTransIds(), task.NodeType, task.ExtendedTime, DueDateTime, ExtendedDateTime);

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("更新工作项实例表失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            MsgDeal("更新工作项实例表成功！：", MsgType.Success);
//            #endregion

//            #region 插入
//            if (intReturn == 0)
//            {
//                //modified by yangpeng 为了避免插入空的时间
//                #region 生成插入sql语句
//                var endTime = taskInstance.GetInstanceEndTime();
//                if (endTime==null||endTime==DateTime.MinValue)
//                {
//                    sSql = string.Format(@"
//	    insert into  wf_TaskInst
//	    (
//	    IDTaskInst,IDProcessInst,IDTask,VersionNo,TaskInstName,Description,ActorID,
//	    StartTime,DueTime,TaskState,CreatePerson,CreateDate,UpdatePerson,UpdateDate,
//        ArrivedTransIDs,NodeType,ExtendedTime,DueDateTime,ExtendedDateTime
//	    )
//	    Values
//	    (
//	    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'
//	    )"
//                      , taskInstance.GetInstanceId(), pi.GetId(), task.Name, pd.GetVersion(), task.Name, task.Description, taskInstance.GetInstanceActors(),
//                       taskInstance.GetInstanceStartTime(), task.DueTime, taskInstance.GetInstanceStatus(), userName, DateTime.Now, taskInstance.GetArrivedTransIds(),
//                       task.NodeType, task.ExtendedTime, DueDateTime, ExtendedDateTime
//                       );
//                } 
//                else
//                {
//                    sSql = string.Format(@"
//	insert into  wf_TaskInst
//	(
//	IDTaskInst,IDProcessInst,IDTask,VersionNo,TaskInstName,Description,ActorID,
//	StartTime,EndTime,DueTime,TaskState,CreatePerson,CreateDate,UpdatePerson,UpdateDate,ArrivedTransIDs,NodeType,ExtendedTime
//    ,DueDateTime,ExtendedDateTime
//	)
//	Values
//	(
//	'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'
//	)"
//                      , taskInstance.GetInstanceId(), pi.GetId(), task.Name, pd.GetVersion(), task.Name, task.Description, taskInstance.GetInstanceActors(),
//                       taskInstance.GetInstanceStartTime(), taskInstance.GetInstanceEndTime(), task.DueTime, taskInstance.GetInstanceStatus(), userName, DateTime.Now,
//                       taskInstance.GetArrivedTransIds(), task.NodeType, task.ExtendedTime, DueDateTime, ExtendedDateTime
//                       );
//                }
//                #endregion

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("插入工作项实例表失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                //待办工作插入到消息表
//                if (taskInstance.GetTask().NodeType == NodeType.Task)
//                    saveWaitWorkMsg(taskInstance);

//                MsgDeal("插入工作项实例表成功！：", MsgType.Success);
//            }
//            #endregion
//            #endregion

//            #region 保存工作实例变量
//            IDictionary<string, string> taskVars = task.TaskVars;
//            foreach (var variable in taskVars)
//            {
//                object o = taskInstance.GetTaskVariable(variable.Key);
//                #region 更新
//                sSql = string.Format(@"
//update wf_Variable 
//set 
//    ValueType='{0}',Value='{1}',UpdatePerson='{2}',UpdateDate='{3}'
//where InstType='{4}'
//and InstTypeValue='{5}'
//and Name='{6}'
//                    ", variable.Value, o, userName, DateTime.Now, InstanceType.TaskInstance, taskInstance.GetInstanceId(), variable.Key);

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("更新工作项实例表的变量失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("更新工作项实例表的变量成功！：", MsgType.Success);
//                #endregion

//                #region 插入
//                if (intReturn == 0)
//                {
//                    sSql = string.Format(@"
//insert into wf_Variable
//(
//    Name,ValueType,Value,InstType,InstTypeValue,CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{5}','{6}'
//)
//                        ", variable.Key, variable.Value, o, InstanceType.TaskInstance, taskInstance.GetInstanceId(), userName, DateTime.Now);
//                    sMsg = _da.execNonQuery(sSql, CommandType.Text, ref  intReturn);

//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("插入工作项实例表的变量失败：" + sMsg, MsgType.Error);
//                        return;
//                    }
//                }

//                MsgDeal("插入工作项实例表的变量成功！", MsgType.Success);
//                #endregion
//            }

//            MsgDeal("保存工作项实例表的变量成功！", MsgType.Success);
//            #endregion

//            #region 保存工作项实例参与人
//            Dictionary<int, string[]> PooledActors = taskInstance.GetPooledActors();
//            foreach(var item in PooledActors)
//            {
//                #region 更新
//                sSql = String.Format(@" 
//update  wf_Assign 
//set     UpdatePerson='{3}',UpdateDate ='{4}'
//where   IDTaskInst = '{0}' and Type ='{1}'and value ='{2}'",
//                       taskInstance.GetInstanceId(), item.Value[0], item.Value[1], userName, DateTime.Now);

//                 sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("更新工作项实例参与人表失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("更新工作项实例参与人表成功！：", MsgType.Success);
//                #endregion

//                #region 插入
//                if (intReturn == 0)
//                {
//                    sSql = string.Format(@"
//insert into  wf_Assign
//(
//    IDTaskInst,Type,value,
//    CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{3}','{4}'
//)"
//                      ,taskInstance.GetInstanceId(), item.Value[0], item.Value[1], userName, DateTime.Now);

//                    sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                    if (sMsg.Length > 0)
//                    {
//                        MsgDeal("插入工作项实例参与人表失败：" + sMsg, MsgType.Error);
//                        return;
//                    }

//                    MsgDeal("插入工作项实例参与人表成功！：", MsgType.Success);
//                }
//                #endregion
//            }
            
//            MsgDeal("保存工作项实例表的参与人成功！", MsgType.Success);
//            #endregion
//        }

//        //保存待办提醒信息
//        private void saveWaitWorkMsg(TaskInstance taskInst)
//        {
//            string sMsg = "";
//            string sSql = "";
//            string MsgContent = "";
//            string MsgTemplete = "";
//            string MsgKind = "WaitWork";
//            string MsgShowKind = "";
//            int intReturn = 0;
//            DataTable dt = new DataTable();
//            string UserIDs = "";
//            string RoleIDs = "";
//            string OrgIDs = "";
//            DataTable dtUsers = new DataTable();

//            #region 循环拼接用户信息字符串
//            foreach (var item in taskInst.GetPooledActors())
//            {
//                string type = item.Value[0];
//                string value = item.Value[1];
//                switch (type)
//                { 
//                    case "User":
//                        UserIDs += value + ",";
//                        break;
//                    case "Role":
//                        RoleIDs += value + ",";
//                        break;
//                    case "Org":
//                        OrgIDs += value + ",";
//                        break;
//                }
//            }
//            #endregion

//            #region 取得需提醒的用户
//            sSql = string.Format(@"
//select * 
//from SysUserInfo
//where 1=0"
//                );
//            if (UserIDs.Trim(',').Length > 0)
//            {
//                sSql += string.Format(" or IDUser in ({0})", UserIDs.Trim(','));
//            }
//            if (RoleIDs.Trim(',').Length > 0)
//            {
//                sSql += string.Format(" or IDRole in ({0})", RoleIDs.Trim(','));
//            }
//            if (OrgIDs.Trim(',').Length > 0)
//            {
//                sSql += string.Format(" or IDOrganize in ({0})", OrgIDs.Trim(','));
//            }

//            sMsg = _da.execQuery(sSql, ref dtUsers);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("读取待办提示消息用户失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            MsgDeal("读取待办提示消息用户成功！" + sMsg, MsgType.Success);
//            #endregion

//            #region 读取模板
//            sSql = string.Format(@"
//select MsgTemplete,MsgShowKind
//from wf_MsgKind
//where MsgKind ='{0}'
//"
//                , MsgKind);

//            sMsg = _da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("读取待办提示消息模板失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            if (dt.Rows.Count > 0)
//            {
//                MsgTemplete = HOLDConvert.ToString(dt.Rows[0]["MsgTemplete"]);
//                MsgShowKind = HOLDConvert.ToString(dt.Rows[0]["MsgShowKind"]);
//            }

//            MsgDeal("读取待办提示消息模板成功！", MsgType.Success);

//            #endregion

//            #region 工作项备注
//            sSql = string.Format(@"
//select Remark
//from wf_TaskInst
//where IDTaskInst ='{0}'
//            "
//                , taskInst.GetInstanceId());

//            sMsg = _da.execQuery(sSql, ref dt);

//            if (sMsg.Length > 0)
//            {
//                MsgDeal("读取工作项备注失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            if (dt.Rows.Count > 0)
//            {
//                //MsgTemplete = HOLDConvert.ToString(dt.Rows[0]["MsgTemplete"]);
//                //MsgShowKind = HOLDConvert.ToString(dt.Rows[0]["MsgShowKind"]);
//                MsgContent = HOLDConvert.ToString(dt.Rows[0]["Remark"]);
//            }

//            MsgDeal("读取工作项备注成功！", MsgType.Success);

//            #endregion

//            #region 循环插入信息表
//            sSql = "";
//            string taskName = taskInst.GetTask().Name;
//            string startTime = HOLDConvert.ToDateTime(taskInst.GetInstanceStartTime(), DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");

//            //if (MsgTemplete.Length > 0)
//            //    MsgContent = string.Format(MsgTemplete, taskName, startTime, _currentuserinfo.UserName);
//            //else
//            //    MsgContent = string.Format(@"{0}-{1}-{2}", taskName, startTime, _currentuserinfo.UserName);
 
//            foreach (DataRow drUser in dtUsers.Select())
//            {
//                sSql += string.Format(@"
//insert into  wf_Msg
//(
//    MsgKind,MsgTitle,MsgContent,MsgShowKind,MsgAlertPeople,State,MsgRelatedKey,MsgRelatedUrl,IDTaskInst,IDProcessInst,
//    CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{10}','{11}'
//)"
//                 , MsgKind, taskName, MsgContent, MsgShowKind, drUser["IDUser"], MsgState.Wait, taskInst.GetInstanceId(), taskInst.GetTask().Form, taskInst.GetInstanceId(), taskInst.GetProcessInstance().GetId(),
//                  _currentuserinfo.UserName, DateTime.Now
//                  );
//            }

//            if (sSql.Length > 0)
//            {
//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("循环插入待办提示消息表失败：" + sMsg, MsgType.Error);
//                    return;
//                }
//            }
//            #endregion

//            MsgDeal("插入待办提示消息表成功！", MsgType.Success);
//        }

//        //获取到期时间、超期时间
//        private string GetDueAndExtendedDateTime(int DueTime, int ExtendedTime, DateTime StartTime)
//        {  
//            string sStr = "";
//            string sMsg = ""; 
//            DateTime DueDateTime = StartTime; //到期时间
//            DateTime ExtendedDateTime = StartTime; //超期时间 
//            DataTable dtDue = new DataTable(); 
//            DataTable dtExtended = new DataTable();
//            string RtnString = ""; //返回值

//            #region 获取到期 时间
//            if (DueTime == 0)
//            {
//                #region 获取系统参数的默认到期天数
//                sStr = string.Format(@" select ControlValue from SysControl
//                                    where ControlCode='DueTime' 
//                                 ");
//                sMsg = _da.execQuery(sStr, ref dtDue);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("检索表数据失败：" + sMsg, MsgType.Error);
//                    return RtnString;
//                }
//                if (dtDue.Rows.Count > 0)
//                {
//                    DueTime = HOLDConvert.ToInt(dtDue.Rows[0]["ControlValue"], 0);
//                }

//            }
//                #endregion

//            #region 计算具体的到期时间
//            for (int i = 1; i <= DueTime; i++)
//            {
//                DueDateTime = DueDateTime.AddDays(1);
//                string weekday = HOLDConvert.ToString(DueDateTime.DayOfWeek);
//                switch (weekday)
//                {
//                    case "Saturday": 
//                        DueDateTime = DueDateTime.AddDays(2);
//                        break;
//                    case "Sunday": 
//                        DueDateTime = DueDateTime.AddDays(1);
//                        break;
//                    default:
//                        break;
//                }
//            }
//             #endregion

//            #endregion

//            #region 获取超期 时间
//            if (ExtendedTime == 0)
//            {
//                #region 获取系统参数的默认超期天数
//                sStr = string.Format(@" select ControlValue from SysControl
//                                    where ControlCode='ExtendedTime' 
//                                 ");
//                sMsg = _da.execQuery(sStr, ref dtExtended);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("检索表数据失败：" + sMsg, MsgType.Error);
//                    return RtnString;
//                }
//                if (dtExtended.Rows.Count > 0)
//                {
//                    ExtendedTime = HOLDConvert.ToInt(dtExtended.Rows[0]["ControlValue"], 0);
//                }
//                #endregion

//            }
//            #region 计算具体的超期时间
//            for (int i = 1; i <= ExtendedTime; i++)
//            {
//                ExtendedDateTime = ExtendedDateTime.AddDays(1);
//                string weekday = HOLDConvert.ToString(ExtendedDateTime.DayOfWeek);
//                switch (weekday)
//                {
//                    case "Saturday": 
//                        ExtendedDateTime = ExtendedDateTime.AddDays(2); ;
//                        break;
//                    case "Sunday": 
//                        ExtendedDateTime = ExtendedDateTime.AddDays(1); ;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            #endregion

//            #endregion

//            RtnString = DueTime.ToString() + ";" + ExtendedTime.ToString() + ";" + DueDateTime.ToString() + ";" + ExtendedDateTime.ToString();
//            return RtnString;
//        } 

//        #endregion

//        #region 保存弧线实例
//        public void save(TransitionInstance transitionInst)
//        {
//            string sSql = "";
//            string sMsg = "";
//            int intReturn = 0;
//            string userName = _currentuserinfo.UserName;
//            TaskInstance fromTaskInst = transitionInst.GetFromTaskInst();
//            string IDFromTaskInst = "";
//            if (fromTaskInst != null) IDFromTaskInst = fromTaskInst.GetInstanceId();

//            DataTable dt = new DataTable();

//            #region 更新
//            sSql = String.Format(@" 
//update  wf_TransitionInst 
//set     IDTaskInst = '{1}', IsExcuted = '{2}', IDTransition = '{3}',
//        UpdatePerson = '{4}', UpdateDate = '{5}'
//where   IDTransitionInst = '{0}' ",
//                     transitionInst.GetId(), IDFromTaskInst, transitionInst.GetIsExecuted(), transitionInst.GetTransition().GetId(),
//                     userName, DateTime.Now);

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("更新弧线实例失败：" + sMsg, MsgType.Error);
//                return;
//            }
            
//            MsgDeal("更新弧线实例成功！", MsgType.Success);
//            #endregion

//            #region 插入
//            if (intReturn == 0)
//            {
//                sSql = String.Format(@" 
//insert into wf_TransitionInst 
//(
//    IDTransitionInst,IDTaskInst,IsExcuted,IDTransition,
//    CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}','{4}','{5}'
//)
//                    ",
//                     transitionInst.GetId(), IDFromTaskInst, transitionInst.GetIsExecuted(), transitionInst.GetTransition().GetId(),
//                     userName, DateTime.Now);

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("插入弧线实例失败：" + sMsg, MsgType.Error);
//                    return;
//                }
//                MsgDeal("插入弧线实例成功！", MsgType.Success);
//            }
//            #endregion

//            MsgDeal("保存弧线实例成功！", MsgType.Success);
//        }
//        #endregion

//        #region 保存Token令牌
//        public void save(Token token)
//        {
//            string sSql = "";
//            string sMsg = "";
//            int intReturn = 0;
//            string userName = _currentuserinfo.UserName;

//            DataTable dt = new DataTable();
//            TaskInstance taskinst = token.GetTaskInstance();
//            ProcessInstance pi = taskinst.GetProcessInstance();

//            #region 更新
//            sSql = String.Format(@" 
//update  wf_Token 
//set     VersionNo = '{1}' , StartTime = '{2}' , EndTime = '{3}' ,
//        IDTaskInst = '{4}' ,ParentIDToken = '{5}' , IDProcessInst = '{6}' ,
//        IsSuspended = '{7}' ,UpdatePerson = '{8}' , UpdateDate = '{9}',TokenState='{10}'
//where   IDToken = '{0}' ",
//                     token.GetId(), pi.GetProcessDefinition().GetVersion(),
//                     taskinst.GetInstanceStartTime(), taskinst.GetInstanceEndTime(),
//                     taskinst.GetInstanceId(), (token.GetParent() == null) ? null : token.GetParent().GetId(), pi.GetId(),
//                     taskinst.GetInstanceStatus() == Zephyr.WorkFlow.TaskState.Pending ? true : false, userName, DateTime.Now, token.GetTokenState());

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("更新令牌表失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            MsgDeal("更新令牌表成功！", MsgType.Success);
//            #endregion

//            #region 插入
//            if (intReturn == 0)
//            {
//                sSql = String.Format(@" 
//insert into  wf_Token 
//(
//    IDToken,VersionNo,StartTime,EndTime,
//    IDTaskInst,ParentIDToken,IDProcessInst,
//    IsSuspended,CreatePerson,CreateDate,UpdatePerson,UpdateDate,TokenState
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{8}','{9}','{10}'
//)
//                    ",
//                     token.GetId(), pi.GetProcessDefinition().GetVersion(),
//                     taskinst.GetInstanceStartTime(), taskinst.GetInstanceEndTime(),
//                     taskinst.GetInstanceId(), (token.GetParent() == null) ? null : token.GetParent().GetId(), pi.GetId(),
//                     taskinst.GetInstanceStatus() == Zephyr.WorkFlow.TaskState.Pending ? true : false, userName, DateTime.Now, token.GetTokenState());

//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("插入令牌表失败" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("插入令牌表成功", MsgType.Success);
//            }
//            #endregion

//            #region 保存MapToken
//            saveMapToken(token);
//            #endregion

//            MsgDeal("保存令牌表成功", MsgType.Success);
//        }

//        //保存MapToken
//        private void saveMapToken(Token JoinToken)
//        {
//            string sSql = "";
//            string sMsg = "";
//            int intReturn = 0;
//            string userName = _currentuserinfo.UserName;

//            DataTable dt = new DataTable();
//            Dictionary<Token, List<Token>> joinMapToken = JoinToken.GetJoinTokenMap();

//            #region 更新
//            sSql = String.Format(@" 
//delete  wf_MapToken 
//where   CurrentIDToken = '{0}' ",
//                     JoinToken.GetId());

//            sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//            if (sMsg.Length > 0)
//            {
//                MsgDeal("删除Join关联令牌表失败：" + sMsg, MsgType.Error);
//                return;
//            }

//            MsgDeal("删除Join关联令牌表成功！", MsgType.Success);
//            #endregion

//            #region 插入
//            sSql = "";
//            foreach (var item in joinMapToken)
//            {
//                var parent = item.Key;
//                var arrives = item.Value;

//                foreach (var i in arrives)
//                {
//                    sSql += String.Format(@" 
//insert into  wf_MapToken 
//(
//    CurrentIDToken,ParentIDToken,ArriveIDToken,
//    CreatePerson,CreateDate,UpdatePerson,UpdateDate
//)
//Values
//(
//    '{0}','{1}','{2}','{3}','{4}','{3}','{4}'
//)
//                    ",
//                    JoinToken.GetId(), parent.GetId(), i.GetId(), userName, DateTime.Now);
//                }
//            }

//            if (sSql.Length > 0)
//            {
//                sMsg = _da.execNonQuery(sSql, CommandType.Text, ref intReturn);
//                if (sMsg.Length > 0)
//                {
//                    MsgDeal("循环插入Join关联令牌表失败：" + sMsg, MsgType.Error);
//                    return;
//                }

//                MsgDeal("循环插入Join关联令牌表成功！", MsgType.Success);
//            }

//            #endregion

//            MsgDeal("保存Join关联令牌表成功！", MsgType.Success);
//        }

//        #endregion
//        #endregion

//        #region Msg处理
//        public void MsgDeal(string sMsg, string msgType)
//        {
//            Msg.Type = msgType;
//            Msg.Message = sMsg;

//            switch (msgType)
//            {
//                case MsgType.Success:
//                    SuccessDeal(sMsg);
//                    break;
//                case MsgType.Error:
//                    ErrorDeal(sMsg);
//                    break;
//                case MsgType.Waning:
//                    WarnningDeal(sMsg);
//                    break;
//                default:
//                    break;
//            }
//        }
//        //出错处理
//        private void ErrorDeal(string sMsg)
//        {
//            _da.Rollback();
//            throw new Exception(sMsg);
//        }

//        //成功处理
//        private void SuccessDeal(string sMsg)
//        {
//            //LOG
//        }

//        //提醒处理
//        private void WarnningDeal(string sMsg)
//        {
//            //LOG
//        }
//        #endregion

//        #region 关闭上下文实例
//        public void close()
//        {
//            _da.Dispose();
//        }
//        #endregion

        // #endregion

        #endregion
    }
}
