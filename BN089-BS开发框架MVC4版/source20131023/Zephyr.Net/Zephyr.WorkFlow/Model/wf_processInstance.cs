using System;
using System.Collections.Generic;
using System.Text;

namespace Zephyr.WorkFlow
{
    public class wf_processInstance : IModel
    {
        public int Id { get; set; }
        public int ProcessDefinitionId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ProcessState { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
