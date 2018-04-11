using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_buttonService : ServiceBase<sys_button>
    {
        protected override bool OnBeforEditDetail(EditEventArgs arg)
        {
            var ButtonCode = arg.row["ButtonCode"].ToString();

            if (arg.type == OptType.Del)
            {
                db.Sql(@"
--É¾³ý½ÇÉ«²Ëµ¥°´Å¥Map
delete sys_roleMenuButtonMap 
where ButtonCode = @0

--É¾³ý²Ëµ¥°´Å¥Map
delete sys_menuButtonMap 
where ButtonCode =@0 ", ButtonCode).Execute();
            }

            return base.OnBeforEditDetail(arg);
        }
    }
    
    public class sys_button : ModelBase
    {

        [PrimaryKey]
        public string ButtonCode
        {
            get;
            set;
        }

        public string ButtonName
        {
            get;
            set;
        }

        public int? ButtonSeq
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ButtonIcon
        {
            get;
            set;
        }

        public string CreatePerson
        {
            get;
            set;
        }

        public DateTime? CreateDate
        {
            get;
            set;
        }

        public string UpdatePerson
        {
            get;
            set;
        }

        public DateTime? UpdateDate
        {
            get;
            set;
        }

    }
}
