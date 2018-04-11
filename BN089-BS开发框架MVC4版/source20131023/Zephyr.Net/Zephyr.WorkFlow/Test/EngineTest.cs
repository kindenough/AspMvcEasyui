using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zephyr.WorkFlow;
using System.Reflection;

namespace Zephyr.WorkFlow
{
    public class BpmTest
    {
        public BpmTest() 
        {
            //BpmConfiguration_Test();
            BpmContext_Test();
        }

        public void BpmConfiguration_Test()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var xml = assembly.GetManifestResourceStream("Zephyr.WorkFlow.Engine.BpmConfiguration.xml");
            BpmConfiguration bpmConfiguration0 = new BpmConfiguration(xml);

            //获取BPM默认配置
            BpmConfiguration bpmConfiguration1 = BpmConfiguration.CreateInstance();

            var config = @"G:\01.开发框架\BS\Zcloud.MVC_2.0\ZCloud.Web\Zephyr.Framework\Zephyr.WorkFlow\Engine\BpmConfiguration.xml";
            BpmConfiguration bpmConfiguration2 = new BpmConfiguration(config);
        }

        public void BpmContext_Test()
        {
            //获取BPM配置
            BpmConfiguration bpmConfiguration = BpmConfiguration.CreateInstance();

            //获取BPM上下文
            BpmContext bpmContext = bpmConfiguration.CreateBpmContext();
            bpmContext.SetCurrentActor("lhs");


            //发布工作流
            var path = @"G:\01.开发框架\BS\Zcloud.MVC_2.0\ZCloud.Web\Zephyr.Framework\Zephyr.WorkFlow\Test\TestFlow.xml";
            ProcessDefinition pd = bpmContext.deployProcessDefinition(path);

            //获取流程实例
            //var processDefinitionName = "主流程";
            //ProcessInstance processInstance1 = bpmContext.newProcessInstance(processDefinitionName);

            var processInstanceId = 1;
            ProcessInstance processInstance2 = bpmContext.loadProcessInstance(processInstanceId);

            processInstance2.start();
             
            ////获取工作项定义
            //var actorId = "lhs";
            //IList<Task> taskList1 = bpmContext.getTaskList(actorId);
            ////IList<Task> taskList2 = bpmContext.getTaskList();

            //IList actors = new ArrayList();
            //actors.Add("lhs");
            //actors.Add("yrh");
            //IList taskList3 = bpmContext.getGroupTaskList(actors);

            ////获取工作项实例
            //var taskInstanceId = "1";
            //TaskInstance taskInstance1 = bpmContext.loadTaskInstance(taskInstanceId);
            ////TaskInstance taskInstance2 = bpmContext.loadTaskInstanceForUpdate(taskInstanceId); //没实现

            ////获取令牌
            //var tokenId = "1";
            //Token token1 = bpmContext.loadToken(tokenId);
            ////Token token2 = bpmContext.loadTokenForUpdate(tokenId);

           

            ////保存工作项实例
            //TaskInstance updateTaskInstance = taskInstance1;
            //bpmContext.save(updateTaskInstance);

            ////保存令牌
            //Token updateToken = token1;
            //bpmContext.save(token1);

            ////保存流程实例
            //ProcessInstance updateProcessInstance = processInstance1;
            //bpmContext.save(updateProcessInstance);
        }

        public void ProcessDefinition_Test()
        {
            BpmConfiguration bpmConfiguration = BpmConfiguration.CreateInstance();

            //获取BPM上下文
            BpmContext bpmContext = bpmConfiguration.CreateBpmContext().SetCurrentActor("lhs");

            //发布流程定义
            var path = "D:\\main.xml";
            var processDefinition1 = bpmContext.deployProcessDefinition(path);

            //加载流程定义
            var processDefinitionId = 1;
            var processDefinition2 = bpmContext.loadProcessDefinition(processDefinitionId);



            //获取流程定义
           
            //ProcessDefinition processDefinition1 = ProcessDefinition.LoadProcessDefinition(path);

            //var processDefinitionId = "PD001";
            // ProcessDefinition processDefinition2 = ProcessDefinition.LoadProcessDefinitionById(processDefinitionId); 注释2013-01-28

            //获取Id
            //var Id = processDefinition1.getProcessDefinitionModel().Id;

            //创建流程实例
            //ProcessInstance processInstance = processDefinition1.CreateProcessInstance();

        }

        public void ProcessInstance_Test()
        {
            ////创建流程实例
            //ProcessDefinition processDefinition = ProcessDefinition.LoadProcessDefinition("D:\\a.xml");
            //ProcessInstance processInstance1 = processDefinition.CreateProcessInstance();
            //ProcessInstance processInstance2 = processDefinition.CreateProcessInstance();

            ////获取流程实例
            //BpmConfiguration bpmConfiguration = BpmConfiguration.CreateInstance();
            //BpmContext bpmContext = bpmConfiguration.CreateBpmContext();
            //var processInstanceId = "1";
            //ProcessInstance processInstance3 = bpmContext.loadProcessInstance(processInstanceId);

            ////流程实例 开始
            //processInstance1.Start();

            ////流程实例 挂起
            //processInstance1.Suspend();

            ////流程实例 继续
            //processInstance1.Resume();

            ////流程实例 结束
            //processInstance1.End();

            ////给流程实例设置变量
            //processInstance1.SetVariable("ProjectId", "P0001");
            //processInstance1.SetVariable("ProjectManager", "zfg");
            //processInstance1.SetVariable("UpdateDate", DateTime.Now);

            ////取得流程实例中的变量
            //var ProjectId = processInstance1.GetVariable("ProjectId");
            //var ProjectManager = processInstance1.GetVariable("ProjectManager");
            //var UpdateDate = processInstance1.GetVariable("UpdateDate");

            ////获取Id
            //var Id = processInstance1.GetId();

            ////获取当前流程实例的令牌
            //Token token = processInstance1.GetRootToken();

            ////获上下文
            //var bpmContext1 = processInstance1.GetContextInstance();

            ////保存工作项实例
            //bpmContext1.save(processInstance1);
        }

        public void TaskIntance_Test()
        {
            ////创建工作项实例
            //ProcessDefinition processDefinition = ProcessDefinition.LoadProcessDefinition("D:\\a.xml");
            //ProcessInstance processInstance = processDefinition.CreateProcessInstance();
            //Task task = processDefinition.GetTaskByName("TaskName");
            //var taskInstance = new TaskInstance(processInstance, task);

            ////获取工作项实例
            //BpmConfiguration bpmConfiguration = BpmConfiguration.CreateInstance();
            //BpmContext bpmContext = bpmConfiguration.CreateBpmContext();
            //var taskInstanceId = "1";
            //TaskInstance taskInstance1 = bpmContext.loadTaskInstance(taskInstanceId);

            ////获取工作项定义
            //Task task1 = taskInstance1.GetTask();

            ////获取xxx
            //var taskId = taskInstance.GetInstanceId();
            //var processInstance1 = taskInstance.GetProcessInstance();
            //var actors = taskInstance.GetInstanceActors();
            //var bizKey = taskInstance.GetInstanceBizKey();

            //var Time1 = taskInstance.GetInstanceCreateTime();
            //var Time2 = taskInstance.GetInstanceEndTime();
            //var TIme3 = taskInstance.GetInstanceStartTime();

            //var formValue = taskInstance.GetInstanceFormValue();
            //var status = taskInstance.GetInstanceStatus();

            ////给工作项实例设置所有者
            //IList actors1 = new ArrayList();
            //actors1.Add("lhs");
            //actors1.Add("yrh");
            //taskInstance.SetInstanceActors(1);

            ////给工作项实例设置变量
            //taskInstance.SetTaskVariable("DoPerson", "zfg");
            //taskInstance.SetTaskVariable("DoDate", DateTime.Now);

            ////工作项开始
            //taskInstance.Start();

            ////工作项结束
            //taskInstance.End();
        }

        public void Token_Test()
        {
            ////创建令牌
            //ProcessDefinition processDefinition = ProcessDefinition.LoadProcessDefinition("D:\\a.xml");
            //ProcessInstance processInstance = processDefinition.CreateProcessInstance();
            //Task startTask = processDefinition.GetStartTask();
            //TaskInstance taskInstance = new TaskInstance(processInstance, startTask);
            //Token token1 = new Token(taskInstance);


            ////获取令牌 从实例中
            //Token token2 = processInstance.GetRootToken();

            ////获取令牌 从数据库中 byId
            //BpmConfiguration bpmConfiguration = BpmConfiguration.CreateInstance();
            //BpmContext bpmContext = bpmConfiguration.CreateBpmContext();
            //var tokenId = "1";
            //Token token3 = bpmContext.loadToken(tokenId);

            ////发出信号
            //token1.Signal();
        }
    }
}
