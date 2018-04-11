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
    public class WarehouseController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new{
                },
                urls = new{
                    query = "/api/psi/warehouse",
                    newkey = "/api/psi/warehouse/getnewkey",
                    edit = "/api/psi/warehouse/edit"//audit = "/api/psi/audit";
                },
                resx = new{
                    noneSelect = "请先选择一条仓库！",
                    editSuccess = "保存成功！",
                    auditSuccess = "已审核！"
                },
                form = new{
                    Id = "",
                    Name = "",
                    ChargePerson = "",
                    Tel = ""
                },
                defaultRow = new {
                    
                },
                setting = new{
                    postListFields = new string[] { "Id", "Name", "ChargePerson", "Tel", "Addr", "Remark" }
                }
            };

            return View(model);
        }

        public ActionResult Stock()
        {
            var model = new
            {
                dataSource = new
                {
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text")),
                    stateItems = new sys_codeService().GetDynamicList(ParamQuery.Instance().Select("value,text").AndWhere("CodeType", "StockState"))
                },
                urls = new
                {
                    query = "/api/psi/warehouse/getstocklist",
                    newkey = "/api/psi/warehouse/getnewkey",
                    edit = "/api/psi/warehouse/edit"//audit = "/api/psi/audit";
                },
                resx = new
                {
                    noneSelect = "请先选择一条库存记录！",
                    editSuccess = "保存成功！",
                    auditSuccess = "已审核！"
                },
                form = new
                {
                    Id = "",
                    Name = "",
                    ChargePerson = "",
                    Tel = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    postListFields = new string[] { "Id", "Name", "ChargePerson", "Tel", "Addr", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class WarehouseApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='Id'>
    <select>*</select>
    <from>psi_warehouse</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Id'            cp='equal'></field>
        <field name='Name'          cp='like' ></field>
        <field name='Warehourse'    cp='equal'></field>
        <field name='Catagory'      cp='equal'></field>
        <field name='Brand'         cp='equal'></field>
    </where>
</settings>");
            var service = new psi_warehouseService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new psi_warehouseService().GetNewKey("Id", "maxplus").PadLeft(3, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_warehouse
    </table>
    <where>
        <field name='Id' cp='equal'></field>
    </where>
</settings>");
            var service = new psi_warehouseService();
            var result = service.Edit(null, listWrapper, data);
        }

        public dynamic GetStockList(RequestWrapper query)
        {
            //stockstate :0:安全库存 1：库存即将不足   2：库存不足
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='A.Id'>
    <select>A.*,C.Model,C.Unit,C.Catagory,C.Brand,C.Name as GoodName,
            Case when A.Num>A.WarnStock 
            then 
                case when A.Num*0.8>A.WarnStock
                then '安全库存'
                else '库存即将不足'
                end
            else '库存不足'
            end as StockState
    </select>
    <from>
        psi_warehouseStock as A
        left join psi_warehouse as B ON B.Id=A.Id 
        left join psi_goods as C ON C.Id=A.GoodNo
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='C.Name'    cp='like' variable='GoodName'></field>
        <field name='B.Id'          cp='equal' variable='Id' ></field>
        <field name='B.ChargePerson'    cp='like' variable='ChargePerson'></field>
    </where>
</settings>");
            var service = new psi_warehouseService();
            var pQuery = query.ToParamQuery();

            //if (query["stateItems"] == "true")
            //    pQuery.AndWhere("AuditState", "passed");
            //else
            //    pQuery.AndWhere("isnull(AuditState,'')", "passed", Cp.notequal);

            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}