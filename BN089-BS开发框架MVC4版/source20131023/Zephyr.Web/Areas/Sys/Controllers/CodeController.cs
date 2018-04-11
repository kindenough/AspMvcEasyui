/*************************************************************************
 * 文件名称 ：CodeController.cs                          
 * 描述说明 ：字典控制器类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using Zephyr.Models;
using Zephyr.Core;

namespace Zephyr.Areas.Sys.Controllers
{
    public class CodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class CodeApiController : ApiController
    {
        
        public dynamic GetCodeType(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='Seq,CodeType'>
   <where defaultIgnoreEmpty='true'>
        <field name='CodeType'      cp='equal'></field>
        <field name='CodeTypeName'  cp='like' ></field>
    </where>
</settings>
");
            var result =  new sys_codeTypeService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='Seq'>
   <where>
        <field name='CodeType' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var service = new sys_codeService();
            var result = service.GetModelListWithPaging(request.ToParamQuery());
            return result;
        }

        public string GetNewCode()
        {
            var service = new sys_codeService();
            return service.GetNewKey("Code", "maxplus").PadLeft(3,'0');
        }


        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_code</table>
    <where>
        <field name='Code' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_codeService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void EditCodeType(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_codeType</table>
    <where>
        <field name='CodeType' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_codeTypeService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
