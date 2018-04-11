using System;

namespace Zephyr.WorkFlow
{
    public class TransEventArgs : EventArgs
    {
        public TaskInstance taskInstance;
        public TransitionInstance transitionInstance;
       // public DataAccess da;

        public TransEventArgs(TransitionInstance transitionInstance)
        {
            //this.taskInstance = (TaskInstance)transitionInstance.GetFromTaskInst();
            //this.transitionInstance = transitionInstance;
            //this.da = taskInstance.GetProcessInstance().GetContextInstance().da;
        }
    }
}
