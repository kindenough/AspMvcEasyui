using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public class wf_processDefinition : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string Xml { get; set; }
        public string Status { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Description { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
