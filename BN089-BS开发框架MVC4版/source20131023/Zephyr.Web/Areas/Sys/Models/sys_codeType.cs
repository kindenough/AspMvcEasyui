using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_codeTypeService : ServiceBase<sys_codeType>
    {
        protected override bool OnBeforEditDetail(EditEventArgs arg)
        {
            var variable = arg.wrapper.getVariableName("CodeType");
            var OldCodeType = arg.row[variable].ToString();
            switch (arg.type)
            {
                case OptType.Mod:
                    var NewCodeType = arg.row["CodeType"].ToString();
                    arg.db.Sql("update sys_code set CodeType=@0 where CodeType=@1", NewCodeType, OldCodeType).Execute();
                    break;
                case OptType.Del:
                    arg.db.Delete("sys_code").Where("CodeType", OldCodeType).Execute();
                    break;
            }

            return true;
        }
    }
    
    public class sys_codeType : ModelBase
    {

        [PrimaryKey]
        public string CodeType { get; set; }

        public string CodeTypeName { get; set; }

        public string Description { get; set; }

        public string Seq { get; set; }

        public string CreatePerson { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdatePerson { get; set; }

        public DateTime? UpdateDate { get; set; }


    }
}
