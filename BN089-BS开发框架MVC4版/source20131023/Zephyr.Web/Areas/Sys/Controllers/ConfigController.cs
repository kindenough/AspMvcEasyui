using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Newtonsoft.Json;
using Zephyr.Models;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Zephyr.Web;

namespace Zephyr.Areas.Sys.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /Sys/Config/
        [MvcMenuFilter(false)]
        public ActionResult Index()
        {
            var themes = new List<dynamic>();
            themes.Add(new { text="默认皮肤",value="default"});
            themes.Add(new { text="流行灰",value="gray"});

            var navigations = new List<dynamic>();
            navigations.Add(new { text="横向菜单",value="menubutton"});
            navigations.Add(new { text="手风琴",value="accordion"});
            navigations.Add(new { text="树形结构",value="tree"});
 
            var model = new {
                dataSource = new{
                    themes=themes,
                    navigations=navigations
                },
                form=  new sys_userService().GetCurrentUserSettings()             
            };

            return View(model);
        }
    }

    public class ConfigApiController : ApiController
    {
        // GET api/sys/config
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/sys/config/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sys/config
        public void Post([FromBody]JObject value)
        {
            if (value == null)
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotAcceptable) { Content = new StringContent("配置不可为空") });
          
            var service = new sys_userService();
            service.SaveCurrentUserSettings(value);
        }

        // PUT api/sys/config/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sys/config/5
        public void Delete(int id)
        {
        }
    }
}
