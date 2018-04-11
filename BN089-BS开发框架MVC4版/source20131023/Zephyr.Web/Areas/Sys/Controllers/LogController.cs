using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;
using Zephyr.Web;

namespace Zephyr.Areas.Sys.Controllers
{
    public class LogController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [MvcMenuFilter(false)]
        public ActionResult View(string id)
        {
             ViewBag.log = LogReader.ReadFile(Server.MapPath("~/logs/" + id + ".txt"));
            return View();
        }

        [MvcMenuFilter(false)]
        public FileResult Download(string id)
        {
            var fileStream = LogReader.ReadFileStream(Server.MapPath("~/logs/" + id + ".txt"));
            return File(fileStream, "text/plain", id + ".txt");
        }
    }

    public class LogApiController : ApiController
    {
        public dynamic GetLoginLog(RequestWrapper request) 
        {
            var service = new sys_loginHistoryService();
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='ID desc'>
    <select>
        A.*
    </select>
    <from>
        sys_loginHistory A
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='LoginDate'           cp='daterange' variable='LogDate'  ></field>
    </where>
</settings>");
            return service.GetModelListWithPaging(request.ToParamQuery());
        }

        public dynamic GetOperateLog(RequestWrapper request) 
        {
            var service = new sys_logService();
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='ID desc'>
    <select>
        A.*
    </select>
    <from>
        sys_log A
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Date'           cp='daterange' variable='LogDate'  ></field>
    </where>
</settings>");
            return service.GetModelListWithPaging(request.ToParamQuery());
        }

        public dynamic GetSystemLog(RequestWrapper request) 
        {
            var page = ZConvert.To<int>(request["page"], 1);
            var rows = ZConvert.To<int>(request["rows"], 0);
            var logDate = ZConvert.ToString(request["logdate"]);

            var list = new List<dynamic>();
            var basepath = HttpContext.Current.Server.MapPath("/logs/");
            var di = new DirectoryInfo(basepath);
            if (!di.Exists) di.Create();

            string[] s = logDate.Split('到');
            string s1 = "1990-01-01";
            string s2 = DateTime.Now.Date.ToString();
            switch (s.Length)
            { 
                case 1:
                    if (logDate.Length > 0)
                    {
                        s1 = s[0];
                        s2 = s1;
                    }
                    break;
                case 2:
                    s1 = s[0];
                    s2 = s[1];
                    break;
            }

            int t1 = Convert.ToInt32(Convert.ToDateTime(s1).ToString("yyyyMMdd"));
            int t2 = Convert.ToInt32(Convert.ToDateTime(s2).ToString("yyyyMMdd"));

            foreach (var fi in di.GetFiles().Where(x => (Convert.ToInt32(x.FullName.Replace(basepath, "").Substring(3,8)) >= t1 && Convert.ToInt32(x.FullName.Replace(basepath, "").Substring(3,8)) <= t2)))
            {
                dynamic item = new ExpandoObject();
                item.filename = fi.FullName.Replace(basepath, "");
                item.size = (fi.Length / 1024).ToString() + " KB";
                item.time = fi.CreationTime.ToString();
                item.id = item.filename.Replace(".txt", "");
                list.Add(item);
            }

            var result = list.OrderByDescending(x => x.filename).Skip((page - 1) * rows).Take(rows);
            return new { rows = result, total = list.Count() };
        }
    }
}
