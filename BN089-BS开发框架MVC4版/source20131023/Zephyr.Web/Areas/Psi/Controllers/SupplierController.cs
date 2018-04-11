using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Psi.Controllers
{
    public class SupplierController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new{
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new{
                    query = "/api/psi/Supplier",
                    newkey = "/api/psi/Supplier/getnewkey",
                    edit = "/api/psi/Supplier/edit"
                },
                resx = new{
                    noneSelect = "请先选择一个供应商！",
                    editSuccess = "保存成功！"
                },
                form = new{
                    Id = "",
                    Name = "",
                    Catagory = "",
                    ChargePerson = ""
                },
                defaultRow = new {
                    
                },
                setting = new{
                    postListFields = new string[] { "Id", "Name", "Catagory", "ChargePerson", "Tel", "Addr", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class SupplierApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='Id'>
    <select>*</select>
    <from>psi_supplier</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Id'            cp='equal'></field>
        <field name='Name'          cp='like' ></field>
        <field name='ChargePerson'  cp='like'></field>
        <field name='Catagory'      cp='equal'></field>
    </where>
</settings>");
            var service = new psi_supplierService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new psi_supplierService().GetNewKey("Id", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_supplier
    </table>
    <where>
        <field name='Id' cp='equal'></field>
    </where>
</settings>");
            var service = new psi_supplierService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}