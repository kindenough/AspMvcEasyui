using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public class wf_transitionInstance : IModel
    {
        public int Id { get; set; }
        public int? FromTaskInstanceId { get; set; }
        public int? ToTaskInstanceId { get; set; }
        public string TransitionId { get; set; }
        public bool? IsExcuted { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
