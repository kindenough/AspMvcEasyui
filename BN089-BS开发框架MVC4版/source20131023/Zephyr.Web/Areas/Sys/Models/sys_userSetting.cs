using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    public class sys_userSettingService : ServiceBase<sys_userSetting>
    {
       
    }

    public class sys_userSetting : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID{ get; set; }
        public string UserCode{ get; set; }
        public string SettingCode{ get; set; }
        public string SettingName{ get; set; }
        public string SettingValue{ get; set; }
        public string Description { get; set; }
    }
}
