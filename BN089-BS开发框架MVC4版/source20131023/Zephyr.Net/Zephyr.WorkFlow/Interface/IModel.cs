using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zephyr.WorkFlow;
using Zephyr.WorkFlow;

namespace Zephyr.WorkFlow
{
    public interface IModel
    {
        int Id { get; set; }
        string CreatePerson { get; set; }
        DateTime? CreateDate { get; set; }
        string UpdatePerson { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
