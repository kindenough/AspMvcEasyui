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

namespace Zephyr.Areas.Mms.Controllers
{
    public class MaterialController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }
    }

    public class MaterialApiController : ApiController
    {
        public dynamic GetTypes(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialType'>
   <where defaultIgnoreEmpty='true'>
        <field name='MaterialType'      cp='equal'></field>
        <field name='MaterialsTypeName'  cp='like' ></field>
    </where>
</settings>
");
            var result = new mms_materialTypeService().GetDynamicList(request.ToParamQuery());
            return result;
        }

        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
   <where>
        <field name='MaterialType' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var service = new mms_materialService();
            var result = service.GetModelListWithPaging(request.ToParamQuery());
            return result;
        }

        public string GetNewCode(string id)
        {
            var service = new mms_materialService();
            return service.GetNewMaterialCode(id);
        }


        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_material</table>
    <where>
        <field name='MaterialCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_materialService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void EditType(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_materialType</table>
    <where>
        <field name='MaterialType' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_materialService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
