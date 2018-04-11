using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public class wf_taskInstance : IModel
    {
        public int Id { get; set; }
        public int ProcessInstanceId { get; set; }
        public string Task { get; set; }
        public string TaskState { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? DueTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatePerson { get; set; }
    }
}
