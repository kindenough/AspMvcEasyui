
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class DepartmentController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/Department",
                    remove = "/api/Mms/Department/",
                    billno = "/api/Mms/Department/getnewbillno",
                    audit = "/api/Mms/Department/audit/",
                    edit = "/Mms/Department/edit/"
                },
                resx = new {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                }
            };

            return View(model);
        }
    }

    public class DepartmentApiController : ApiController
    {
        

        public List<dynamic> Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='DEPTID'>
    <select>*</select>
    <from>REMP_Department</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
    </where>
</settings>");
            var service = new REMP_DepartmentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
