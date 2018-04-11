
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class LiyController : Controller
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
                    query = "/api/Mms/Liy",
                    newkey = "/api/Mms/Liy/getnewkey",
                    edit = "/api/Mms/Liy/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    DepartmentID = "" ,
                    IsValid = "" ,
                    ApproveState = "" ,
                    Remark = "" ,
                    OutDateTime = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "ID",
                    postListFields = new string[] { "ID" ,"DepartmentID" ,"IsValid" ,"OutDateTime" ,"ApproveState" ,"ApprovePerson" ,"ApproveDate" ,"ApproveRemark" ,"Remark" }
                }
            };

            return View(model);
        }
    }

    public class LiyApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>test_liy</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='DepartmentID'		cp='equal'></field>   
        <field name='IsValid'		cp='equal'></field>   
        <field name='ApproveState'		cp='equal'></field>   
        <field name='Remark'		cp='like'></field>   
        <field name='OutDateTime'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new test_liyService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new test_liyService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        test_liy
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new test_liyService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
