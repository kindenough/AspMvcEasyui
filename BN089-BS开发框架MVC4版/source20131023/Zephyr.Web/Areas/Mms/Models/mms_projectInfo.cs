using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
	[Module("Mms")]
    public class mms_projectService : ServiceBase<mms_project>
    {
       
    }

    public class mms_project : ModelBase
    {

        [PrimaryKey]
        public string ProjectCode{ get; set; }
        public string ProjectName{ get; set; }
        public string ShortName{ get; set; }
        public string OwnerUnit{ get; set; }
        public string DesignUnit{ get; set; }
        public string ConstructionUnit{ get; set; }
        public string SupervisionUnit{ get; set; }
        public string ChargePerson{ get; set; }
        public decimal? ContractMoney{ get; set; }
        public DateTime? ContractBeginDate{ get; set; }
        public DateTime? ContractEndDate{ get; set; }
        public DateTime? ActualBeginDate{ get; set; }
        public DateTime? ActualEndDate{ get; set; }
        public decimal? ActualDuration{ get; set; }
        public string KeyProject{ get; set; }
        public string FundSource{ get; set; }
        public string Region{ get; set; }
        public string ProjectArea { get; set; }
        public string BuildingType{ get; set; }
        public string ProjectExplain{ get; set; }
        public decimal? BuildingArea{ get; set; }
        public decimal? CompleteArea{ get; set; }
        public decimal? ProjectCost{ get; set; }
        public string CreatePerson{ get; set; }
        public DateTime? CreateDate{ get; set; }
        public string UpdatePerson{ get; set; }
        public DateTime? UpdateDate{ get; set; }
        public string Remark{ get; set; }
        public string ParentCode{ get; set; }
    }
}
