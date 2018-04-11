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
    public class GoodsController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new{
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new{
                    query = "/api/psi/goods",
                    newkey = "/api/psi/goods/getnewkey",
                    edit = "/api/psi/goods/edit"//audit = "/api/psi/audit";
                },
                resx = new{
                    noneSelect = "请先选择一条货物数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    Id = "",
                    Name = "",
                    Catagory = "",
                    Brand = ""
                },
                defaultRow = new {
                    
                },
                setting = new{
                    postListFields = new string[] { "Id", "Name", "Catagory", "Brand", "Model", "Unit", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class GoodsApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='Id'>
    <select>*</select>
    <from>psi_goods</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Id'            cp='equal'></field>
        <field name='Name'          cp='like' ></field>
        <field name='Warehourse'    cp='equal'></field>
        <field name='Catagory'      cp='equal'></field>
        <field name='Brand'         cp='equal'></field>
    </where>
</settings>");
            var service = new psi_goodsService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new psi_goodsService().GetNewKey("Id", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_goods
    </table>
    <where>
        <field name='Id' cp='equal'></field>
    </where>
</settings>");
            var service = new psi_goodsService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}