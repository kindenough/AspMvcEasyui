
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
    public class ProjectController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = new{
                    query = "/api/mms/Project",
                    newkey = "/api/mms/Project/getnewkey",
                    edit = "/api/mms/Project/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    ParentCode = "" ,
                    ShortName = "" ,
                    ProjectName = "" ,
                    OwnerUnit = "" ,
                    DesignUnit = "" ,
                    ConstructionUnit = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "ProjectCode",
                    postListFields = new string[] { "ParentCode" ,"DesignUnit" ,"SupervisionUnit" ,"ContractMoney" ,"OwnerUnit" ,"ShortName" ,"ProjectName" ,"ConstructionUnit" ,"ChargePerson" ,"ContractBeginDate" ,"ActualBeginDate" }
                };
            };

            return View(model);
        }

    }

    public class ProjectApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ProjectCode'>
    <select>*</select>
    <from>mms_project</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ParentCode' cp='equal'></field>   
        <field name='ShortName' cp='equal'></field>   
        <field name='ProjectName' cp='equal'></field>   
        <field name='OwnerUnit' cp='equal'></field>   
        <field name='DesignUnit' cp='equal'></field>   
        <field name='ConstructionUnit' cp='equal'></field>   
    </where>
</settings>");
            var service = new mms_projectService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_projectService().GetNewKey("ProjectCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_project
    </table>
    <where>
        <field name='ProjectCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_projectService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
