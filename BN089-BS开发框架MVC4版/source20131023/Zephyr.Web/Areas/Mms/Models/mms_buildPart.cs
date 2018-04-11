using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_buildPartService : ServiceBase<mms_buildPart>
    {
        public dynamic GetBuildPartItems(string projectCode = null)
        {
            var pQuery = ParamQuery.Instance()
                .Select("BuildPartCode as value,BuildPartName as text")
                .AndWhere("ProjectCode", projectCode);

            return base.GetDynamicList(pQuery);
        }
    }

    public class mms_buildPart : ModelBase
    {

        [PrimaryKey]
        public string BuildPartCode{ get; set; }
        [PrimaryKey]
        public string ProjectCode{ get; set; }
        public string ParentCode { get; set; }
        public string BuildPartName{ get; set; }
        public string PartAttr{ get; set; }
        public string NodeControl{ get; set; }
        public DateTime? ActualBeginTime{ get; set; }
        public DateTime? ActualEndTime{ get; set; }
        public string ImagePart{ get; set; }
        public string Remark{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
    }
}
