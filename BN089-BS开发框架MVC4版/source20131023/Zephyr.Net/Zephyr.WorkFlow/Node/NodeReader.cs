using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;
using System;

namespace Zephyr.WorkFlow
{
    public class NodeReader  
    {
        public string _name;               //名称
        public string _version;            //版本
        public string _description;        //描述
        public int _startNodeIndex;        //开始结点
        public int _endNodeIndex;          //终止结点
        public IList<Task> _nodes;         //结点列表
        public Node _rootElement;          //XML所有节点结构
        public IDictionary<string, int> _taskNameMap;  //工作项对应Map
        public IDictionary<string, string> _processVars;  //流程变量
        public string _bizKeyName;

        public NodeReader(ProcessDefinition processDefinition, string xmlstring)
        {
            #region 读取xml处理
            try
            {
                //初始化
                _startNodeIndex = -1;
                _endNodeIndex = -1;
                _rootElement = null;
                _nodes = new List<Task>();
                _taskNameMap = new Dictionary<string, int>();

                StringReader ts = new StringReader(xmlstring);
                var _xmlreader = new XmlTextReader(ts);

                _rootElement = null;
                var _elementStack = new ArrayList();

                while (_xmlreader.Read())
                {
                    switch (_xmlreader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:                      //xml声明信息
                            break;
                        case XmlNodeType.Element:                             //元素信息
                            Node newElement = new Node(_xmlreader.Name);
                            bool isEmptyElement = _xmlreader.IsEmptyElement;

                            Hashtable attributes = new Hashtable();
                            if (_xmlreader.HasAttributes)
                            {
                                for (int i = 0; i < _xmlreader.AttributeCount; i++)
                                {
                                    _xmlreader.MoveToAttribute(i);
                                    attributes.Add(_xmlreader.Name, _xmlreader.Value);
                                }
                            }

                            newElement.Attributes = attributes;

                            //元素堆栈为空时，为根节点（流程节点），不为空时，为子节点（任务节点）
                            int elementStackSize = _elementStack.Count;
                            if (elementStackSize > 0)
                            {
                                Node containingElement = (Node)_elementStack[_elementStack.Count - 1];
                                containingElement.AddChild(newElement);
                            }
                            else
                            {
                                _rootElement = newElement;
                                _name = newElement.GetAttribute("name");    //取得流程的信息(名称+版本)
                                _version = newElement.GetAttribute("version");
                                _description = newElement.GetAttribute("description");
                                _bizKeyName = newElement.GetAttribute("bizkey");
                            }

                            _elementStack.Add(newElement);

                            if (isEmptyElement)
                            {
                                _elementStack.RemoveAt(_elementStack.Count - 1);
                            }
                            break;
                        case XmlNodeType.EndElement:                                //结束节点
                            _elementStack.RemoveAt(_elementStack.Count - 1);
                            break;
                        case XmlNodeType.Text:                                      //普通文本
                            if (_xmlreader.Value.Length > 0)
                            {
                                Node element = (Node)_elementStack[_elementStack.Count - 1];
                                element.AddText(_xmlreader.Value);
                            }
                            break;
                    }
                }

                //将节点的所有属性信息存在RootElement中
                var contents = _rootElement.Content;
                var length = contents.Count;

                //循环存储Task的信息
                for (var i = 0; i < length; i++)
                {
                    Node node = (Node)contents[i];

                    switch (node.Name.ToLower())
                    {
                        case "controller":
                            #region controller
                            //node.AddChild()
                            foreach (var item in node.GetChildElements("variable"))
                            {
                                Node varNode = (Node)item;
                                _processVars.Add(varNode.GetAttribute("name"), varNode.GetAttribute("type"));
                            }
                            #endregion
                            break;
                        default:
                            #region taskAttribute
                            Task task = new Task();

                            //int Duetime = HOLDConvert.ToInt(BLLSysControl.GetParam("Duetime"), 0);      //到期默认天数
                            //int  Extendedtime = HOLDConvert.ToInt(BLLSysControl.GetParam("Extendedtime"),0); //超期默认天数

                            task.Index = i;                                             //index
                            task.Name = node.GetAttribute("name");                      //名称
                            //task.Description = node.GetAttribute("description");        //描述
                            task.Version = _version;                            //版本
                            ////task.IsBlocking = HOLDConvert.ToBool(node.GetAttribute("Blocking"));       //是否阻塞
                            ////task.DueTime = HOLDConvert.ToInt(node.GetAttribute("duetime"), 0);          //到期 天数天数
                            ////task.ExtendedTime = HOLDConvert.ToInt(node.GetAttribute("extendedtime"), 0);    //超期天数
                            ////task.IsAssign = (node.GetAttribute("isassign") == null) ? true : HOLDConvert.ToBool(node.GetAttribute("isassign"));
                            ////task.IsAutoStart = (node.GetAttribute("isautostart") == null) ? false : HOLDConvert.ToBool(node.GetAttribute("isautostart"));

                            //task.NodeType = Enum.EunmConvert.ToNodeType(node.Name);         //节点类型
                            //task.LogicType = Enum.EunmConvert.ToLogicType(node.GetAttribute("logictype"));        //逻辑类型

                            task.PDefinition = processDefinition;                            //流程定义
                            //task.ReturnParent = (node.GetAttribute("returnparent") == null) ? false : HOLDConvert.ToBool(node.GetAttribute("returnparent"));
                            //...todo

                            //子流程相关
                            task.SubFlowName = node.GetAttribute("subflowname");
                            task.SubFlowVersion = node.GetAttribute("subflowversion");

                            foreach (var item in node.GetChildElements("form"))
                            {
                                Node formnode = (Node)item;
                                task.Form = formnode.GetAttribute("value");
                                //task.FromType = Enum.EunmConvert.ToFormType(formnode.GetAttribute("type"));
                            }

                            foreach (var itemControl in node.GetChildElements("controller"))
                            {
                                Node ControlNode = (Node)itemControl;
                                foreach (var item in ControlNode.GetChildElements("variable"))
                                {
                                    Node varNode = (Node)item;
                                    task.TaskVars.Add(varNode.GetAttribute("name"), varNode.GetAttribute("type"));
                                }
                            }


                            //string Form{get;set;}                   //表单
                            //string FormDefaultValue{get;set;}       //表单默认值
                            //FormType FromType{get;set;}             //表单类型

                            _nodes.Add(task);
                            _taskNameMap.Add(task.Name, task.Index);

                            //标志开始和结束节点的位置
                            if (node.Name == "start")
                            {
                                _startNodeIndex = i;
                            }
                            else if (node.Name == "end")
                            {
                                _endNodeIndex = i;
                            }
                            #endregion
                            break;
                    }
                }

                if (_startNodeIndex < 0 || _endNodeIndex < 0)
                {
                    throw new Exception("缺少开始结束节点");
                }

                #region 循环存储来源和去向节点，动作
                for (var i = 0; i < length; i++)
                {
                    Node node = (Node)contents[i];
                    var transitions = node.GetChildElements("transition");
                    foreach (var item in transitions)
                    {
                        var transNode = (Node)item;
                        var toIndex = _taskNameMap[transNode.GetAttribute("to")];
                        var fromNode = _nodes[i];
                        var toNode = _nodes[toIndex];

                        var transition = new Transition();
                        transition.From = fromNode;
                        transition.To = toNode;
                        transition.Condition = transNode.GetAttribute("condition");

                        var events = transNode.GetChildElements("action");
                        foreach (var ievent in events)
                        {
                            var actionNode = (Node)ievent;
                            var action = new EventAction();
                            action.Name = actionNode.Name;
                            action.Class = actionNode.GetAttribute("class");
                            action.Method = actionNode.GetAttribute("method");
                            action.EventType = EunmConvert.ToEventType(actionNode.GetAttribute("event"));
                            action.ActionType = EunmConvert.ToActionType(actionNode.GetAttribute("type"));
                            action.VarNames = actionNode.GetAttribute("variable");
                            transition.Event.Add(action);
                        }

                        fromNode.LeavingTransitions.Add(transition);
                        toNode.ArrivingTransitions.Add(transition);
                    }

                    var actions = node.GetChildElements("action");
                    foreach (var item in actions)
                    {
                        var actionNode = (Node)item;
                        var action = new EventAction();
                        action.Name = actionNode.Name;
                        action.Class = actionNode.GetAttribute("class");
                        action.Method = actionNode.GetAttribute("method");
                        action.EventType = EunmConvert.ToEventType(actionNode.GetAttribute("event"));
                        action.ActionType = EunmConvert.ToActionType(actionNode.GetAttribute("type"));
                        action.VarNames = actionNode.GetAttribute("variable");
                        _nodes[i].Actions.Add(action);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public Task getTaskByName(string name)//通过任务名称取得节点
        {
            var index = _taskNameMap[name];
            return _nodes[index];
        }

        public IList<Task> getTaskList() //取得流程定义所有节点
        {
            return _nodes;
        }

        public Task getStartTask() //取得开始节点
        {
            return _nodes[_startNodeIndex];
        }

        public Task getEndTask()//取得结束节点
        {
            return _nodes[_endNodeIndex];
        }
    }
}
