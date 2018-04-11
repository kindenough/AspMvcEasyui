using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class TestController : Controller
    {
        //
        // GET: /Mms/Test/

        public ActionResult Index()
        {
            var currentProject = MmsHelper.GetCurrentProject();
            var model = new
            {
                urls = MmsHelper.GetIndexUrls("test"),
                resx = MmsHelper.GetIndexResx("大数据测试单"),
                form = new
                {
                    UserCode = "",
                    UserName = "",
                    Description = "",
                    IsEnable = "",
                    LoginCount = "",
                    LastLoginDate = ""
                }
            };

            return View(model);
        }

        public ActionResult BigData()
        {
            return View();
        }

    }

    public class TestApiController : ApiController
    {
        // 查询主表：GET api/mms/send
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='UserCode'>
    <select>
        *
    </select>
    <from>
        test_user
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserCode'             cp='like'      ></field>
        <field name='UserName'             cp='like'      ></field>
        <field name='Description'             cp='like'      ></field>
        <field name='IsEnable'             cp='equal'      ></field>
        <field name='LoginCount'             cp='equal'      ></field>
        <field name='LastLoginDate'             cp='daterange'      ></field>
    </where>
</settings>");

            var pQuery = query.ToParamQuery();
            var list = new mms_sendService().GetDynamicListWithPaging(pQuery);
            return list;
        }

        //[System.Web.Http.HttpPost]
        //public dynamic PostFile()
        //{
        //    var aa = this.Request;

        //    // 设置上传目录
        //   // var provider = new MultipartFormDataStreamProvider(@"D:\");
        //    // 接收数据，并保存文件
        //  // Request.Content.ReadAsMultipartAsync(provider);

        //    var context = HttpContext.Current;
        //    var request = context.Request;

        //    var path = context.Server.MapPath("/Upload/");
        //    request.Files[0].SaveAs(path + request.Files[0].FileName);
 
        //    return new { status=1 };
        //}

        [System.Web.Http.HttpPost]
        public dynamic PostFile()
        {
            // 设置上传目录
            // var provider = new MultipartFormDataStreamProvider(@"D:\");
            // 接收数据，并保存文件
            // Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                var context = HttpContext.Current;
                var request = context.Request;
 
                //保存文件
                var postFile = request.Files[0];
                string uploadPath = HttpContext.Current.Server.MapPath("~/Upload/");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                string filePath = postFile.FileName.Substring(postFile.FileName.LastIndexOf("\\") + 1);
                string fileType = filePath.Substring(filePath.LastIndexOf("."));
                filePath = filePath.Substring(0, filePath.LastIndexOf("."));
                filePath = uploadPath + filePath + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond + fileType;
                request.Files[0].SaveAs(filePath);

                //读取文件
                var dt = new DataTable();
                var msg = ReadFile(filePath,ref dt);

                if (!string.IsNullOrEmpty(msg))
                {
                    throw new Exception(msg);
                }
           
                using (var db = Db.Context("Mms"))
                {
                    db.UseTransaction(true);
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var user = new sys_user();
                            user.UserCode = row[0].ToString();
                            user.UserName = row[1].ToString();
                            user.Description = row[2].ToString();
                            user.IsEnable = row[3].ToString().ToLower() == "true";

                            int count;
                            if (!int.TryParse(row[4].ToString(), out count))
                            {
                                count = 0;
                            }

                            DateTime date;
                            if (!DateTime.TryParse(row[5].ToString(), out date))
                            {
                                date = DateTime.Now;
                            }

                            user.LoginCount = count;
                            user.LastLoginDate = date;


                            var ret = db.Sql("select 1 from test_user where UserCode =@0", user.UserCode).QuerySingle<int>();

                            if (ret <= 0)
                            {
                                db.Insert<sys_user>("test_user", user).AutoMap().Execute();
                            }
                            else
                            {
                                db.Update<sys_user>("test_user", user).AutoMap(x => x.UserCode).Where(x => x.UserCode).Execute();
                            }

                        }

                        db.Commit();
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        throw ex;
                    }
                }
            }
            catch(Exception e)
            {
                return new { error = e.Message, preventRetry = true };
            }

            //返回前台
            return new { success = true, message = "导入成功!" };
        }


        public Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            // 检查该请求是否含有multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            // Read the form data and return an async task.
            // 读取表单数据，并返回一个async任务
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }

                    // This illustrates how to get the file names.
                    // 以下描述了如何获取文件名
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        Trace.WriteLine("Server file path: " + file.LocalFileName);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                });

            return task;
        }

        //读取数据
        private string ReadFile(string filePath,ref DataTable dtSource)
        {
            string strCon = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1'", filePath);
            System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection(strCon);
            try
            {
                Conn.Open();
                var Sheet1 = "Sheet1";
                string sSheetName = "";

                if (String.IsNullOrEmpty(Sheet1))
                {
                    List<string> lstSheetNames = new List<string>();
                    DataTable dtSheets = new DataTable();
                    dtSheets = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    for (int i = 0; i < dtSheets.Rows.Count; i++)
                    {
                        if (!dtSheets.Rows[i]["TABLE_NAME"].ToString().Contains("_"))
                        {
                            lstSheetNames.Add(dtSheets.Rows[i]["TABLE_NAME"].ToString());
                        }
                    }
                    sSheetName = lstSheetNames[0];
                }
                else
                {
                    sSheetName = Sheet1 + "$";
                }

                DataSet ds = new DataSet();
                string strCom = String.Format("SELECT * FROM [{0}]", sSheetName);
                System.Data.OleDb.OleDbDataAdapter myCommand = new System.Data.OleDb.OleDbDataAdapter(strCom, Conn);
                myCommand.Fill(ds, "ExcelTable");
                dtSource = ds.Tables[0];
            }
            catch(Exception e)
            {
                return "读取文件数据时出错:" + e.Message;
            }
            finally
            {
                Conn.Close();
                try
                {
                    FileInfo fi = new FileInfo(filePath); //删除临时文件
                    fi.Delete();
                }
                catch{}
            }

            return "";
        }
    
    }
}
