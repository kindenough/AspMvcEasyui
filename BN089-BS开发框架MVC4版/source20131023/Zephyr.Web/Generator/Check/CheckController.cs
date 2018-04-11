
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class CheckController : Controller
    {
        var userName = MmsHelper.GetUserName();
        var currentProject = MmsHelper.GetCurrentProject();
        var data = new ReceiveApiController().GetEditMaster(id);
        var codeService = new sys_codeService();

        var model = new
        {
            form = data.form,
            scrollKeys = data.scrollKeys,
            urls = new {
                getdetail =  "/api/mms/receive/getdetail/",            //获取明细数据api 
                getmaster =  "/api/mms/receive/geteditmaster/",        //获取主表数据及数据滚动数据api
                edit =  "/api/mms/receive/edit/",                      //数据保存api
                audit =  "/api/mms/receive/audit/",                    //审核api
                getrowid =  "/api/mms/receive/getnewrowid/"            //获取新的明细数据的主键(日语叫采番)
            },
            resx = new {
                rejected = "已撤消修改！",
                editSuccess = "保存成功！",
                auditPassed ="单据已通过审核！",
                auditReject = "单据已取消审核！"
            },
            dataSource = new{
                measureUnit = codeService.GetMeasureUnitListByType(),
                supplyType = codeService.GetValueTextListByType("SupplyType"),
                payKinds = codeService.GetValueTextListByType("PayType"),
                warehouseItems = new mms_warehouseService().GetWarehouseItems(currentProject)
            },
            defaultForm = new mms_receive().Extend(new {  //定义主表数据的默认值
                BillNo = id,
                BillDate = DateTime.Now,
                DoPerson = userName,
                ReceiveDate = DateTime.Now,
                SupplyType = codeService.GetDefaultCode("SupplyType"),
                PayKind = codeService.GetDefaultCode("PayType"),
            }),
            defaultRow = new {                           //定义从表数据的默认值
                 CheckNum = 1,
                Num = 1,
                UnitPrice = 0,
                Money = 0,
                PrePay = 0
            },
            setting = new
            {
                postFormKeys = new string[] { "BillNo" },              //主表的主键
                  postListFields = new string[] { "BillNo", "RowId",     //定义从表中哪些字段要传递到后台
                                 "MaterialCode", "Unit", "CheckNum", "Num", "UnitPrice", "PrePay", "Money", "Remark" }
            }
        };
        return View(model);
    }

    public class CheckApiController : ApiController
    {
        // GET api/mms/send/geteditmaster 取得编辑页面中的主表数据及上一页下一页主键
         public dynamic GetEditMaster(string id) {
            var projectCode = MmsHelper.GetCookies("CurrentProject");
            var masterService = new mms_receiveService();
            return new{
                form = masterService.GetModel(ParamQuery.Instance().AndWhere("BillNo", id)),
                scrollKeys = masterService.ScrollKeys("BillNo", id, ParamQuery.Instance().AndWhere("ProjectCode", projectCode))
            };
        }
 
        // 地址：GET api/mms/send/getnewrowid 预取得新的明细表的行号
         public string GetNewRowId(int id)
        {
            var service = new mms_receiveDetailService();
            return service.GetNewKey("RowId", "maxplus",id);
        }
 
        // 地址：GET api/mms/send/getdetail 功能：取得收料单明细信息
         public dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo",id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
    <select>
        A.*, B.MaterialName,B.Model,B.Material
    </select>
    <from>
        mms_receiveDetail A
        left join mms_materialInfo B on B.MaterialCode = A.MaterialCode
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var ReceiveService = new mms_receiveService();
            var result = ReceiveService.GetDynamicListWithPaging(pQuery);
            return result;
        }
  
        // 地址：POST api/mms/send 功能：保存收料单数据
         [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        mms_receive
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>");

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <columns ignore='MaterialName,Model,Material'></columns>
    <table>
        mms_receiveDetail
    </table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>");
             
            var service = new mms_receiveService();
            var result = service.Edit(formWrapper, listWrapper, data);
        }
    }
}
