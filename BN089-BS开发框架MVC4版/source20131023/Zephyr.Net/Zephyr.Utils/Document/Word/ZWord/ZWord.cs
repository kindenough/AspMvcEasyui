using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web;
using System.Collections;
using System.IO;

namespace Zephyr.Utils
{
    public class ZWord
    {
        /// <summary>
        /// 把DataTable导出为Word文件
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="fileName">Word文件名(不包括后缀*.doc)</param>
        /// <param name="dtbl">将要被导出的DataTable对象</param>
        /// <returns></returns>
        public static bool DataTableToWord(System.Web.HttpResponse response, string fileName, DataTable dtbl)
        {
            response.Clear();
            response.Buffer = true;
            response.Charset = "UTF-8";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".doc");
            response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            response.ContentType = "application/ms-word";
            //page.EnableViewState = false;
            response.Write(DataTableToHtmlTable(dtbl));
            response.End();
            return true;
        }

        public static Stream ListToHtmlTable(object list, Dictionary<string, string> titles, bool IsExportAllCol)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            sBuilder.Append("<tr>");
            EachHelper.EachListHeader(list, (index, name, type) => 
            {
                if (IsExportAllCol || titles.ContainsKey(name))
                    sBuilder.Append(string.Format("<td>{0}</td>", titles[name]??name));
            });
            sBuilder.Append("</tr>");
            EachHelper.EachListRow(list, (index, row) =>
            {
                sBuilder.Append("<tr>");
                EachHelper.EachObjectProperty(row, (cellIndex, name, value) => {
                    if (IsExportAllCol || titles.ContainsKey(name))
                        sBuilder.Append(string.Format("<td>{0}</td>", value ?? string.Empty));
                });
                sBuilder.Append("</tr>");
            });
            sBuilder.Append("</table");

            byte[] byteArray = Encoding.Default.GetBytes(sBuilder.ToString());
            var stream = new MemoryStream(byteArray);
            return stream;
        }
 
        /// <summary>
        /// 把DataTable转换成Html的Table
        /// </summary>
        /// <param name="dataTable">DataTable对象</param>
        /// <returns></returns>
        public static string DataTableToHtmlTable(DataTable dataTable)
        {
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            foreach (DataRow dr in dataTable.Rows)
            {
                sBuilder.Append("<tr>");
                foreach (DataColumn dc in dataTable.Columns)
                {
                    if (dc.ColumnName.Equals(""))
                    {
                        sBuilder.Append(string.Format("<td>{0}</td>", dr[dc].ToString()));
                    }
                    else
                    {
                        sBuilder.Append(string.Format("<td>{0}</td>", dr[dc].ToString()));// style='vnd.ms-excel.numberformat:@'
                    }
                }
                sBuilder.Append("</tr>");
            }
            sBuilder.Append("</table");
            return sBuilder.ToString();
        }
    }
}

