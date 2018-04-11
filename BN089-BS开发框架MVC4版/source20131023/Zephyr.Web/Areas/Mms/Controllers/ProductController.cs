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
    public class ProductController : Controller
    {
        //
        // GET: /Mms/TestABC/

        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    UnitItems = new sys_codeService().GetDynamicList(ParamQuery.Instance().Select("Code as value,Text as text").AndWhere("CodeType", "MeasureUnit"))
                },
                urls = new
                {
                    query = "/api/mms/product",
                    newkey = "/api/mms/product/getnewkey",
                    edit = "/api/mms/product/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条产品数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ProductName = "",
                    Color = "",
                    Price = "",
                    Unit = "",
                    Remark ="",
                    CreateDate = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    postListFields = new string[] { "ID", "ProductName", "Color", "Price", "Unit", "Money", "Qty", "Remark", "CreateDate" }
                }
            };

            return View(model);
        }

    }

    public class ProductApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>mms_product</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ProductName'     cp='like'></field>
        <field name='Color'           cp='equal' ></field>
        <field name='Price'           cp='equal'></field>
        <field name='Unit'            cp='equal'></field>
        <field name='Remark'          cp='like'></field>
        <field name='CreateDate'      cp='daterange'></field>
    </where>
</settings>");
            var service = new mms_productService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new mms_productService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_product
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_productService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
