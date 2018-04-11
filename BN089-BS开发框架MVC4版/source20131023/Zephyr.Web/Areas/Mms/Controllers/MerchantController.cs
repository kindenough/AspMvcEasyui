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
    public class MerchantController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }
    }

    public class MerchantApiController : ApiController
    {
        /// <summary>
        /// 地址：GET api/mms/merchant/getnames
        /// 功能：取得收料单供应商
        /// 调用：供应商自动完成
        /// </summary>
        public List<dynamic> GetNames(string q)
        {
            var ReceiveService = new mms_merchantsService();
            var pQuery = ParamQuery.Instance().Select("top 10 MerchantsName").AndWhere("MerchantsName", q, Cp.StartWithPY);
            return ReceiveService.GetDynamicList(pQuery);
        }

        public dynamic GetTypes(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='MerchantsTypeCode'>
   <where defaultIgnoreEmpty='true'>
        <field name='MerchantsTypeCode'      cp='equal'></field>
        <field name='MerchantsTypeName'  cp='like' ></field>
    </where>
</settings>
");
            var result = new mms_merchantsTypeService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='MerchantsCode'>
   <where>
        <field name='MerchantsTypeCode' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var service = new mms_merchantsService();
            var result = service.GetModelListWithPaging(request.ToParamQuery());
            return result;
        }

        public string GetNewCode(string id)
        {
            var service = new mms_merchantsService();
            return service.GetNewKey("MerchantsCode", "maxplus").PadLeft(3, '0');
        }


        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_merchants</table>
    <where>
        <field name='MerchantsCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_merchantsService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void EditType(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>mms_merchantsType</table>
    <where>
        <field name='MerchantsTypeCode' cp='equal'></field>
    </where>
</settings>");
            var service = new mms_merchantsService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
