using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Index(string id)
        {
            if (id == null) id = MmsHelper.GetCurrentProject();
            var data = new ProjectApiController().GetProjectInfo(id);
            var service = new sys_codeService();
            var model = new
            {
                urls = new {
                    newkey = ""
                },
                form = data.form,
                scrollKeys = data.scrollKeys,
                dataSource = new
                {
                    partAttr = service.GetValueTextListByType("PartAttr"),
                    imagePart = service.GetValueTextListByType("ImagePart")
                }
            };
            return View(model);
        }
         
    }

    public class ProjectApiController : ApiController
    {
        public dynamic GetProjectInfo(string id)
        {
            var service = new mms_projectService();
            return new {
                form = service.GetModel(ParamQuery.Instance().AndWhere("ProjectCode", id)),
                scrollKeys = service.ScrollKeys("ProjectCode", id, ParamQuery.Instance().AndWhere("ProjectCode", id))
            };
        }

        public List<dynamic> GetProjectName(string q)
        {
            var service = new mms_projectService();
            var pQuery = ParamQuery.Instance().Select("top 10 ProjectName").AndWhere("ProjectName", q, Cp.StartWithPY);
            return service.GetDynamicList(pQuery);
        }

        public dynamic GetList()
        {
            var service = new mms_projectService();
            var pQuery = ParamQuery.Instance().Select("ProjectName as text,ProjectCode as id,ParentCode as pid");
            return service.GetDynamicList(pQuery);
        }

        public dynamic GetSub(string id)
        {
            return new mms_buildPartService().GetModelList(ParamQuery.Instance().AndWhere("ProjectCode", id));
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_project
    </table>
    <where>
        <field name='ProjectCode' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_buildPart
    </table>
    <where>
        <field name='ProjectCode' cp='equal'></field>
        <field name='BuildPartCode'  cp='equal'></field>
    </where>
</settings>");

            var service = new mms_directService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }

        //新增项目
        public mms_project GetNewProject()
        {
            var service = new mms_projectService();

            var model = new mms_project() {
                ProjectCode = service.GetNewKey("ProjectCode", "maxplus"),
            };

            return model;
        }

        //新增部位
        public mms_buildPart GetNewBuildPart(string projectCode, string parentCode)
        {
            var service = new mms_buildPartService();

            var model = new mms_buildPart()
            {
                ProjectCode = projectCode,
                BuildPartCode = service.GetNewKey("BuildPartCode", "maxplus"),
                ParentCode = parentCode
            };

            return model;
        }
    }
}
