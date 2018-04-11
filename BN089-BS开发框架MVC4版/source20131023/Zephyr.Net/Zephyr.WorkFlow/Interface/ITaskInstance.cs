//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;
//using Zephyr.WorkFlow;
//using Zephyr.WorkFlow;

//namespace Zephyr.WorkFlow
//{
//    public interface TaskInstance
//    {
//        Action<TaskEventArgs> OnEnter { get; set; }//任务开始事件
//        Action<TaskEventArgs> OnLeave { get; set; }//任务结束事件
//        Action<TaskEventArgs> OnAssign { get; set; }
//        Action<TaskEventArgs> OnCreate { get; set; }
//        Action<TaskEventArgs> OnStart { get; set; }
//        Action<TaskEventArgs> OnEnd { get; set; }
//        Action<TaskEventArgs> OnDelete { get; set; }

//        IList GetArrivedTransitions();     //已到达的线

//        Task GetTask();
//        string GetInstanceId();
//        TaskState GetInstanceStatus();
//        string GetInstanceBizKey();
//        int GetInstanceActors();
//        void SetInstanceActors(int actors);
//        DateTime? GetInstanceCreateTime();
//        DateTime? GetInstanceStartTime();
//        DateTime? GetInstanceEndTime();
//        string GetInstanceFormValue();
//        string GetArrivedTransIds();
//        Dictionary<int, string[]> GetPooledActors();

//        void SetTaskVariable(string name, object value);
//        object GetTaskVariable(string name);
//        bool HasEnded();
//        void SetTaskInstState(TaskState taskState);

//        ProcessInstance GetProcessInstance();

//        void Start();
//        void EnterNode();
//        void End();
//        void Delete();
//        TaskInstance CopyTaskInst();

//        void Leave(Token token, TransitionInstance transitionInstance);
//        void Enter(Token token, TransitionInstance transitionInstance);
//    }
//}
