using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_warehouseService : ServiceBase<mms_warehouse>
    {
        public dynamic GetWarehouseItems(string projectCode=null)
        {
            var pQuery = ParamQuery.Instance()
                .Select("WarehouseCode as value,WarehouseName as text")
                .AndWhere("ProjectCode", projectCode);

            return base.GetDynamicList(pQuery);
        }
    }

    public class mms_warehouse : ModelBase
    {

        [PrimaryKey]
        public string WarehouseCode{ get; set; }
        public string ProjectCode{ get; set; }
        public string WarehouseName { get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
    }
}
