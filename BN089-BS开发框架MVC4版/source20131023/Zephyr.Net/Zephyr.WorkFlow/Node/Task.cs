using System.Collections.Generic;
using System.Collections;

namespace Zephyr.WorkFlow
{
    public class Task  
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public int DueTime { get; set; }
        public int ExtendedTime { get; set; }
        public bool IsAssign { get; set; }
        public bool IsAutoStart { get; set; }

        public bool IsBlocking { get; set; }

        public NodeType NodeType { get; set; }
        public LogicType LogicType { get; set; }

        public IList Actions { get; set; } 
        public IList ArrivingTransitions { get; set; }
        public IList LeavingTransitions { get; set; }

        //public XmlElement NodeXml { get; set; }
        public ProcessDefinition PDefinition { get; set; }
        public bool ReturnParent { get; set; }
 
        public string Form { get; set; }
        public string FormDefaultValue { get; set; }
        public FormType FromType { get; set; }

        public IDictionary<string, string> TaskVars { get; set; }

        //public string GetId(){
        //   // return String.Format("{0}-{1}-{2}", this.Name, this.Version, this.Index);
        //}
        //子流程
        public string SubFlowName { get; set; }
        public string SubFlowVersion { get; set; }

        public Task()
        {
            this.NodeType = NodeType.Task;
            this.LogicType = LogicType.OR;
            this.DueTime = 0;
            this.ExtendedTime = 0;

            this.IsBlocking = false;

            this.Actions = new ArrayList();
            this.ArrivingTransitions = new ArrayList();
            this.LeavingTransitions = new ArrayList();

            this.FromType = FormType.Url;
            this.NodeType = NodeType.Task;
            this.LogicType = LogicType.AND;

            this.TaskVars = new Dictionary<string, string>();
        }
 
        public TaskInstance createTaskInstance(ProcessInstance processInstance)
        {
            return new TaskInstance(processInstance, this, null);
        }

        public TaskInstance loadTaskInstance(ProcessInstance processInstance, wf_taskInstance model)
        {
            return new TaskInstance(processInstance, this, model);
        }

        public TaskInstance loadOrCreateTaskInstance(ProcessInstance processInstance)
        {
            if (this.NodeType == NodeType.Join)
            {
                wf_taskInstance dataTaskInstance = null;
                var bpmContext = processInstance.getBpmContext();
                switch (this.LogicType)
                {
                    case LogicType.AND:         //join的Token停留在Temp等待
                        dataTaskInstance = bpmContext.loadDataTaskInstanceFirstRun(processInstance.getProcessInstanceModel().Id, this.Name);
                        return this.loadTaskInstance(processInstance, dataTaskInstance);
                        break;
                    case LogicType.OR:          //join的Token不停留Temp
                        dataTaskInstance = bpmContext.loadDataTaskInstanceFirst(processInstance.getProcessInstanceModel().Id, this.Name);
                        return this.loadTaskInstance(processInstance, dataTaskInstance);
                        break;
                }

            }
            return createTaskInstance(processInstance);
        }
    }
}
