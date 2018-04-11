/*************************************************************************
 * 文件名称 ：XlsExport.cs                          
 * 描述说明 ：导出EXCEL2003
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.IO;
using Zephyr.Utils;
using Zephyr.Utils.NPOI.HSSF.UserModel;
using Zephyr.Utils.NPOI.HSSF.Util;
using Zephyr.Utils.NPOI.SS.UserModel;
using Zephyr.Utils.NPOI.SS.Util;

namespace Zephyr.Core
{
    public class XlsExport:IExport
    {
        public string suffix { get {return "xls"; } }

        private HSSFWorkbook workbook;
        private ISheet sheet;
        private ICellStyle dataStyle;

        public void Init(object data)
        {
            workbook = new HSSFWorkbook();
            sheet = workbook.CreateSheet("sheet1");
            sheet.DefaultRowHeight = 200 * 20;
        }

        public void MergeCell(int x1,int y1,int x2,int y2)
        {
            CellRangeAddress range = new CellRangeAddress(y1, y2, x1, x2);
            sheet.AddMergedRegion(range);  
        }

        public virtual void FillData(int x, int y,string field, object value)
        {
            var row = sheet.GetRow(y) ?? sheet.CreateRow(y);
            var cell = row.GetCell(x) ?? row.CreateCell(x);
 
            if (!field.StartsWith("title_"))
                cell.CellStyle = GetDataStyle();

            switch ((value ?? string.Empty).GetType().Name.ToLower())
            {
                case "int32":
                case "int64":
                case "decimal":
                    cell.CellStyle.Alignment = HorizontalAlignment.RIGHT;
                    cell.SetCellValue(ZConvert.To<double>(value, 0));
                    break;
                default:
                    cell.SetCellValue(ZConvert.ToString(value));
                    break;
            }
        }

        public virtual void SetHeadStyle(int x1, int y1, int x2, int y2)
        {
            var style = GetHeadStyle();
            for (var y = y1; y <= y2; y++)
            {
                var row = sheet.GetRow(y) ?? sheet.CreateRow(y);
                for (var x = x1; x <= x2; x++)
                {
                    var cell = row.GetCell(x) ?? row.CreateCell(x);
                    cell.CellStyle = style;
                }
            }
        }

        public virtual void SetRowsStyle(int x1, int y1, int x2, int y2)
        {
            var style = GetDataStyle();
            for (var y = y1; y <= y2; y++)
            {
                var row = sheet.GetRow(y) ?? sheet.CreateRow(y);
                for (var x = x1; x <= x2; x++)
                {
                    var cell = row.GetCell(x) ?? row.CreateCell(x);
                    cell.CellStyle = style;
                }
            }
        }

        public Stream SaveAsStream()
        {
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            sheet = null;
            return ms;
        }

        private ICellStyle GetHeadStyle()
        {
            //表头样式
            var headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.CENTER;//居中对齐
            headStyle.VerticalAlignment = VerticalAlignment.CENTER;

            //表头单元格背景色
            headStyle.FillForegroundColor = HSSFColor.LIGHT_GREEN.index;
            headStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            //表头单元格边框
            headStyle.BorderTop = BorderStyle.THIN;
            headStyle.TopBorderColor = HSSFColor.BLACK.index;
            headStyle.BorderRight = BorderStyle.THIN;
            headStyle.RightBorderColor = HSSFColor.BLACK.index;
            headStyle.BorderBottom = BorderStyle.THIN;
            headStyle.BottomBorderColor = HSSFColor.BLACK.index;
            headStyle.BorderLeft = BorderStyle.THIN;
            headStyle.LeftBorderColor = HSSFColor.BLACK.index;
            //表头字体设置
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 12;//字号
            font.Boldweight = 600;//加粗
            //font.Color = HSSFColor.WHITE.index;//颜色
            headStyle.SetFont(font);

            return headStyle;
        }

        private ICellStyle GetDataStyle()
        {
            if (dataStyle == null)
            {
                //数据样式
                dataStyle = workbook.CreateCellStyle();
                dataStyle.Alignment = HorizontalAlignment.LEFT;//左对齐
                //数据单元格的边框
                dataStyle.BorderTop = BorderStyle.THIN;
                dataStyle.TopBorderColor = HSSFColor.BLACK.index;
                dataStyle.BorderRight = BorderStyle.THIN;
                dataStyle.RightBorderColor = HSSFColor.BLACK.index;
                dataStyle.BorderBottom = BorderStyle.THIN;
                dataStyle.BottomBorderColor = HSSFColor.BLACK.index;
                dataStyle.BorderLeft = BorderStyle.THIN;
                dataStyle.LeftBorderColor = HSSFColor.BLACK.index;
                //数据的字体
                var datafont = workbook.CreateFont();
                datafont.FontHeightInPoints = 11;//字号
                dataStyle.SetFont(datafont);
            }

            return dataStyle;
        }
    }
}
