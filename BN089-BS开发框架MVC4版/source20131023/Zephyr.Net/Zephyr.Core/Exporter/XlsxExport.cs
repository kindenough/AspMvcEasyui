/*************************************************************************
 * 文件名称 ：XlsxExport.cs                          
 * 描述说明 ：导出EXCEL2007
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Drawing;
using System.IO;
using Zephyr.Utils;
using Zephyr.Utils.EPPlus;
using Zephyr.Utils.EPPlus.Style;

namespace Zephyr.Core
{
    public class XlsxExport:IExport
    {
        public string suffix { get {return "xlsx"; } }
 
        private ExcelPackage package;
        private ExcelWorksheet sheet;

        public void Init(object data)
        {
            package = new ExcelPackage();
            sheet = package.Workbook.Worksheets.Add("sheet1");
        }

        public void MergeCell(int x1,int y1,int x2,int y2)
        {
            sheet.Cells[y1+1, x1+1, y2+1, x2+1].Merge = true;
        }

        public virtual void FillData(int x, int y,string field, object value)
        {
            if (ZGeneric.IsTypeIgoreNullable<DateTime>(value))
                sheet.Cells[y + 1, x + 1].Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
            sheet.Cells[y + 1, x + 1].Value = value;
        }

        public virtual void SetHeadStyle(int x1, int y1, int x2, int y2)
        {
            using (var head = sheet.Cells[y1 + 1, x1 + 1, y2 + 1, x2 + 1]) // set head style
            {
                head.Style.Font.Bold = true;
                head.Style.Font.Size = 12;
                head.Style.Font.Name = "Arial";

                head.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                head.Style.Border.Top.Color.SetColor(Color.Gray);
                head.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                head.Style.Border.Right.Color.SetColor(Color.Gray);
                head.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                head.Style.Border.Bottom.Color.SetColor(Color.Gray);
                head.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                head.Style.Border.Left.Color.SetColor(Color.Gray);

                head.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                head.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                head.Style.Fill.PatternType = ExcelFillStyle.Solid;
                head.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }
        }

        public virtual void SetRowsStyle(int x1, int y1, int x2, int y2)
        {
            using (var data = sheet.Cells[y1 + 1, x1 + 1, y2 + 1, x2 + 1])// set data style
            {
                data.Style.Font.Name = "Arial";
                data.Style.Font.Size = 11;

                data.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                data.Style.Border.Top.Color.SetColor(Color.Gray);
                data.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                data.Style.Border.Right.Color.SetColor(Color.Gray);
                data.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                data.Style.Border.Bottom.Color.SetColor(Color.Gray);
                data.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                data.Style.Border.Left.Color.SetColor(Color.Gray);
            }
        }

        public Stream SaveAsStream()
        {
            var ms = new MemoryStream();
            package.SaveAs(ms);

            package = null;
            sheet = null;
            return ms;
        }
    }
}
