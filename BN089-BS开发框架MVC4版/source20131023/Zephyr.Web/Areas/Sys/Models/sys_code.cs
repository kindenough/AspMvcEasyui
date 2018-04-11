using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_codeService : ServiceBase<sys_code>
    {
        public List<dynamic> GetValueTextListByType(string codeType)
        {
            var pQuery = ParamQuery.Instance()
                .Select("Code as value,Text as text")
                .AndWhere("CodeType", codeType);

            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetMeasureUnitListByType()
        {
            var pQuery = ParamQuery.Instance()
                .Select("Text as value,Text as text")
                .AndWhere("CodeType", "MeasureUnit");

            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetYearItems() {
            var result = new List<dynamic>();
            var startYear = DateTime.Now.Year - 10;
            for (var y = startYear; y < startYear + 20; y++)
                result.Add(new {value=y,text=y });
            
            return result;
        }

        public List<dynamic> GetMonthItems()
        {
            var result = new List<dynamic>();
            var startMonth = 1;
            for (var m = startMonth; m < startMonth + 12; m++)
                result.Add(new { value = m, text = m });
            
            return result;
        }

        public string GetDefaultCode(string sType)
        {
            var pQuery = ParamQuery.Instance().Select("top 1 Code")
                .AndWhere("CodeType", sType)
                .AndWhere("IsEnable",true)
                .AndWhere("IsDefault",true);

            return base.GetField<string>(pQuery);
        }
    }
    
    public class sys_code : ModelBase
    {

        [PrimaryKey]
        public string Code { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        public string ParentCode { get; set; }

        public string Seq { get; set; }

        public bool? IsEnable { get; set; }

        public bool? IsDefault { get; set; }

        public string Description { get; set; }

        public string CodeTypeName { get; set; }

        public string CodeType { get; set; }

        public string CreatePerson { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdatePerson { get; set; }

        public DateTime? UpdateDate { get; set; }



    }
}
