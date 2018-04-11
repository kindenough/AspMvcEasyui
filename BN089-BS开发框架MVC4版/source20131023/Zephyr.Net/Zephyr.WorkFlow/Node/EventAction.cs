using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.WorkFlow;

namespace Zephyr.WorkFlow
{
    public class EventAction
    {
        public string Name { get; set; }
        public EventType EventType { get; set; }
        public ActionType ActionType { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string VarNames { get; set; }
    }
}
