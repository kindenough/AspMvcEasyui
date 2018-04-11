using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public class wf_token : IModel
    {
        public int Id { get; set; }
        public int TaskInstanceId { get; set; }
        public int ProcessInstanceId { get; set; }
        public int? ParentId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TokenState { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
