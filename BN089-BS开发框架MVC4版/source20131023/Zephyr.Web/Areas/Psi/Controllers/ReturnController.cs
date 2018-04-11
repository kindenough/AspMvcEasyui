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
    public class ReturnController : Controller
    {
        #region 查询页面
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    //warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    query = "/api/psi/return",
                    remove = "/api/psi/return/",
                    billno = "/api/psi/return/getnewbillno",
                    audit = "/api/psi/return/audit/",
                    edit = "/psi/return/edit/"
                },
                resx = new
                {
                    detailTitle = "退还单明细",
                    noneSelect = "请先选择一条退还单！",
                    deleteConfirm = "确定要删除选中的退还单吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    BillNo = "",
                    ReturnPerson = "",
                    SupplierName = "",
                    PickingBillNo = "",
                    Contract = "",
                    ReturnDate = ""
                }
            };

            return View(model);
        }
        #endregion

        #region 编辑页面
        public ActionResult Edit(string id = "")
        {
            var data = new ReturnApiController().GetEditMaster(id);
            var model = new
            {
                form = data.form,
                scrollKeys = data.scrollKeys,
                dataSource = new
                {
                    warehouseItems = new psi_warehouseService().GetDynamicList(ParamQuery.Instance().Select("Id as value,Name as text"))
                },
                urls = new
                {
                    getdetail = "/api/psi/return/getdetail/",
                    getmaster = "/api/psi/return/geteditmaster/",
                    edit = "/api/psi/return/edit",
                    audit = "/api/psi/return/audit/",
                    getrowid = "/api/psi/return/getnewrowid/",
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                defaultRow = new
                {
                    Num = 1,
                    UnitPrice = 0,
                    Money = 0
                },
                setting = new
                {
                    postFormKeys = new string[] { "BillNo" },
                    postListFields = new string[] { "BillNo", "RowId", "GoodNo", "Num", "UnitPrice", "Money", "Remark" }
                }
            };

            return View(model);
        }
        #endregion
    }

    public class ReturnApiController : ApiController
    {
        #region 查询页面 api
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>
        A.*
    </select>
    <from>
        psi_return A
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo'            cp='like'      ></field>
        <field name='PickingBillNo'         cp='like'       ></field>
        <field name='ReturnPerson'     cp='like'       ></field>
        <field name='Contract'          cp='like'       ></field>
        <field name='ReturnDate'       cp='daterange'       ></field>
        <field name='CustomNo'            cp='like'></field>
    </where>
</settings>");

            var ReturnService = new psi_returnService();
            var pQuery = query.ToParamQuery();
            var result = ReturnService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewBillNo()
        {
            var service = new psi_returnService();
            return service.GetNewKey("BillNo", "dateplus");
        }

        public void Delete(string id)
        {
            var service = new psi_returnService();
            var result = service.Delete(ParamDelete.Instance().AndWhere("BillNo", id));
            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("退还单删除失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, dynamic data)
        {
            var service = new psi_returnService();
            var result = service.Update(ParamUpdate.Instance()
                .Column("AuditState", data.status)
                .Column("AuditReason", data.comment)
                .Column("AuditPerson", FormsAuth.GetUserData().UserName)
                .Column("AuditDate", DateTime.Now)
                .AndWhere("BillNo", id));

            if (result <= 0)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent("审核退还单失败{BillNo=" + id + "}，请重试或联系管理员！") });
        }
        #endregion

        #region 编辑页面用 api
        public dynamic GetEditMaster(string id)
        {
            dynamic result = new ExpandoObject();
            var masterService = new psi_returnService();
            result.form = masterService.GetModel(ParamQuery.Instance().AndWhere("BillNo", id));
            result.form.BillNo = id; //如果没有数据，返回一条空数据，BillNo=id; 对应新增的情况
            result.scrollKeys = masterService.ScrollKeys("BillNo", id);
            return result;
        }

        public string GetNewRowId(int id)
        {
            var service = new psi_returnGoodsService();
            return service.GetNewKey("RowId", "maxplus", id);
        }

        public dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='RowId'>
    <select>
        A.*, B.Name,B.Catagory,B.Brand,B.Model,B.Unit
    </select>
    <from>
        psi_returnGoods A
        left join psi_goods B on B.Id = A.GoodNo
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var ReturnService = new psi_returnService();
            var result = ReturnService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_return
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        psi_returnGoods
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>");

            var service = new psi_returnService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}