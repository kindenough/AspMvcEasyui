using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Collections;
using System.Web;

namespace Zephyr.WorkFlow
{
    public class ProcessDefinition
    {
        private BpmContext _bpmContext;
        private wf_processDefinition _model;
        private NodeReader _nodeReader;
  
        internal ProcessDefinition(BpmContext bpmContext,wf_processDefinition dataProcessDefinition)
        {
            _bpmContext = bpmContext;
            _nodeReader = new NodeReader(this, dataProcessDefinition.Xml);
            _model = dataProcessDefinition;
        }

        internal ProcessDefinition(BpmContext bpmContext,string xmlString)
        {
            _bpmContext = bpmContext;
            _nodeReader = new NodeReader(this, xmlString);
            _model = new wf_processDefinition() { 
                Xml = xmlString,
                Name = _nodeReader._name,
                Description = _nodeReader._description,
                Version = bpmContext.getProcessDefinitionNextVersion(_nodeReader._name),
                EffectiveDate = DateTime.Now,
                Status = "1"
            };

            _model.Id = bpmContext.save(this);
        }

        public wf_processDefinition getProcessDefinitionModel()
        {
            return _model;
        }

        public Task getTaskByName(string name)
        {
            return _nodeReader.getTaskByName(name);
        }

       
        public IList<Task> getTaskList()
        {
            return _nodeReader.getTaskList();
        }

       
        public Task getStartTask()
        {
            return _nodeReader.getStartTask();
        }
        
        public Task getEndTask()
        {
            return _nodeReader.getEndTask();
        }
 
        public ProcessInstance createProcessInstance()
        {
            return new ProcessInstance(this, null);
        }

        public ProcessInstance loadProcessInstance(wf_processInstance model)
        {
            return new ProcessInstance(this, model);
        }

        public BpmContext getBpmContext()
        {
            return _bpmContext;
        }
    }
}
