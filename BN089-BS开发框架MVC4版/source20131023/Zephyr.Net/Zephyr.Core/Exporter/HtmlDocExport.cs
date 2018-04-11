/*************************************************************************
 * 文件名称 ：HtmlDocExport.cs                          
 * 描述说明 ：导出htmldoc
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zephyr.Core
{
    public class HtmlDocExport:IExport
    {
        public string suffix { get {return "doc"; } }

        private StringBuilder sBuilder;
        private int rowIndex;
        private Dictionary<int,object> row; 

        public void Init(object data)
        {
            rowIndex = 0;
            row = new Dictionary<int, object>();
            sBuilder = new StringBuilder();
            sBuilder.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            sBuilder.Append("<tr>");
        }

        public void MergeCell(int x1,int y1,int x2,int y2)
        {
            throw new Exception("htmldoc未实现多选title");
        }

        public virtual void FillData(int x, int y,string field, object value)
        {
            if (rowIndex < y)
            {
                AppendRow(row.OrderBy(m => m.Key).Select(m => m.Value).ToArray());
                row = new Dictionary<int, object>();
                rowIndex++;
            }

            row[x] = value;
        }

        public virtual void SetHeadStyle(int x1, int y1, int x2, int y2)
        {
           
        }

        public virtual void SetRowsStyle(int x1, int y1, int x2, int y2)
        {
            
        }

        public Stream SaveAsStream()
        {
            AppendRow(row.OrderBy(m => m.Key).Select(m => m.Value).ToArray());
            sBuilder.Append("</table");

            byte[] byteArray = Encoding.Default.GetBytes(sBuilder.ToString());
            var stream = new MemoryStream(byteArray);
            return stream;
        }

        private void AppendRow(object [] values)
        {
            sBuilder.Append("<tr>");
            foreach(var value in values)
                sBuilder.Append(string.Format("<td>{0}</td>", value??string.Empty ));
            sBuilder.Append("</tr>");
        }
    }
}
