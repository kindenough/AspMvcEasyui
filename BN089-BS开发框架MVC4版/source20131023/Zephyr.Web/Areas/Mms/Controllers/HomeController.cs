using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class HomeController : Controller
    {
        //
        // GET: /MMS/Home/
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("~/Login/mms");

            return View();
        }

        public ActionResult LookupMaterial()
        {
            return View();
        }
    }

    public class HomeApiController : ApiController
    {
        //弹出材料选择窗口数据
        public dynamic GetMaterial(RequestWrapper request)
        {
            var service = new mms_materialService();
            return service.GetDynamicListWithPaging(request.ToParamQuery());
        }

        //弹出材料选择窗口数据
        public dynamic GetMaterialType()
        {
            var service = new mms_materialTypeService();
            var requst = RequestWrapper.Instance()
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialType'>
    <select>
        MaterialTypeName as text,MaterialType as id,ParentCode as pid
    </select>
    <from>
        mms_materialType
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
    </where>
</settings>");

            var pQuery = requst.ToParamQuery();

            //pQuery.AndWhere("MaterialType", "", x => {
            //    var data = request.ToParamQuery().GetData();
            //    var from = data.From.IndexOf(' ')>-1 ? string.Format("({0})", data.From) : data.From;
            //    var where = data.WhereSql;
            //    where = string.IsNullOrEmpty(where)? string.Empty:("where " + where);
            //    var sql = string.Format("MaterialType in (select MaterialType from {0} {1})", from, where);
            //    return sql;
            //});

            return service.GetDynamicList(pQuery);
        }
    }
}
