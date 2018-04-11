using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    public class sys_menuButtonMapService : ServiceBase<sys_menuButtonMap>
    {
       
    }

    public class sys_menuButtonMap : ModelBase
    {

        [Identity]
        [PrimaryKey]
        public int ID{ get; set; }
        public string MenuCode{ get; set; }
        public string ButtonCode{ get; set; }
    }
}
