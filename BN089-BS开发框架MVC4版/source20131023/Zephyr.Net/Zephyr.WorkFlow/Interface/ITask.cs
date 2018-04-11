//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;
//using Zephyr.WorkFlow;
//using Zephyr.WorkFlow;

//namespace Zephyr.WorkFlow
//{
//    public interface Task
//    {
//        int Index { get; set; }
//        string Name{get;set;}                   //名称
//        string Description{get;set;}            //描述
//        string Version { get; set; }            //版本
//        bool IsBlocking{get;set;}               //是否阻塞 
//        int DueTime { get; set; }               //时间秒
//        int ExtendedTime { get; set; }
//        bool IsAssign { get; set; }
//        bool IsAutoStart { get; set; }          //子流程是否自动启动

//        NodeType NodeType { get; set; }         //结点类型
//        LogicType LogicType{get;set;}           //逻辑类型

//        IList Actions{get;set;}                 //处理
//        IList ArrivingTransitions{get;set;}     //来源
//        IList LeavingTransitions{get;set;}      //去向

//        //XmlElement NodeXml{get;set;}          //Xml
//        ProcessDefinition PDefinition{get;set;} //Definition
//        bool ReturnParent { get; set; }//子流程输出参数

//        string Form{get;set;}                   //表单
//        string FormDefaultValue{get;set;}       //表单默认值
//        FormType FromType{get;set;}             //表单类型

//        IDictionary<string, string> TaskVars { get; set; }
        
//        //子流程
//        string SubFlowName { get; set; }
//        string SubFlowVersion { get; set; }

//        //string GetId();

//        //to do
//        //ExceptionHandler
//    }
//}
