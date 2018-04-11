using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class TestABCController : Controller
    {
        //
        // GET: /Mms/TestABC/

        public ActionResult Index()
        {
           var model = new
            {
                dataSource = new{
                    TypeItems = new sys_codeService().GetDynamicList(ParamQuery.Instance().Select("Code as value,Text as text").AndWhere("CodeType", "Pricing"))
                },
                urls = new{
                    query = "/api/mms/testabc",
                    newkey = "/api/mms/testabc/getnewkey",
                    edit = "/api/mms/testabc/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条货物数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    Year = "",
                    ProjectName = "",
                    DeclaringUnits = "",
                    ProjectType = "",
                    StartDate="",
                    EndDate =""
                },
                defaultRow = new {
                    
                },
                setting = new{
                    postListFields = new string[] { "ID", "Year", "ProjectName", "DeclaringUnits", "ProjectType", "StartDate", "EndDate","TotalMoney","Remark" }
                }
            };

           return View(model);
        }

    }

    public class TestABCApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>mms_test</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ID'                    cp='equal'></field>
        <field name='ProjectName'           cp='like' ></field>
        <field name='DeclaringUnits'        cp='like'></field>
        <field name='ProjectType'           cp='equal'></field>
        <field name='StartDate'             cp='dtgreaterequal'></field>
        <field name='EndDate'               cp='dtlessequal'></field>
    </where>
</settings>");
            var service = new mms_testService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_testService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_test
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_testService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
