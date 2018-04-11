using System;
using System.Collections;

namespace Zephyr.WorkFlow
{
    public class Transition
    {
        public Task From { get; set; }
        public Task To { get; set; }
        public string Condition { get; set; }
        public IList Event { get; set; }

        public string GetId()
        {
            return String.Format("{0}-{1}-{2}", From.Name, To.Name, From.Version);
        }

        public Transition()
        {
            this.Event = new ArrayList();
            this.From = new Task();
            this.Condition = "";
            this.To = new Task();

        }
 
        public TransitionInstance createTransitionInstance(TaskInstance fromTaskInstance)
        {
            return new TransitionInstance(fromTaskInstance, this, null);
        }

        public TransitionInstance loadTransitionInstance(TaskInstance fromTaskInstance, wf_transitionInstance model)
        {
            return new TransitionInstance(fromTaskInstance, this, model);
        }

    }
}
