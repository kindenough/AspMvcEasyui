using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zephyr.Utils.NPOI.HSSF.UserModel;
using Zephyr.Utils.NPOI.HSSF.Util;
using Zephyr.Utils.NPOI.SS.UserModel;
using System.Collections;

namespace Zephyr.Utils.NPOI
{
    public class NPOIHelper
    {
        public static Stream ListToExcel(object list, Dictionary<string, string> titles, bool IsExportAllCol)
        {
            const int startIndex = 0;
            var fields = titles.Keys.ToList();
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            sheet.DefaultRowHeight = 200 * 20;
            var row = sheet.CreateRow(startIndex);

            var headStyle = GetHeadStyle(workbook);
            EachHelper.EachListHeader(list, (i, name, type) =>
            {
                if (!fields.Contains(name))
                {
                    if (IsExportAllCol)
                        fields.Add(name);
                    else
                        return;
                }
                var cellIndex = fields.IndexOf(name) + startIndex;
                var cell = row.CreateCell(cellIndex);
                cell.SetCellValue(titles.ContainsKey(name)?titles[name]:name);
                cell.CellStyle = headStyle;
                sheet.AutoSizeColumn(cellIndex);
            });
            
            EachHelper.EachListRow(list, (rowIndex, dataRow) => 
            {
                row = sheet.CreateRow(rowIndex + 1);
                EachHelper.EachObjectProperty(dataRow, (i, name, value) => 
                {
                    if (!fields.Contains(name))
                    {
                        if (IsExportAllCol)
                            fields.Add(name);
                        else
                            return;
                    }
                    var cellIndex = fields.IndexOf(name) + startIndex;
                    var dataStyle = GetDataStyle(workbook);
                    var cell = row.CreateCell(cellIndex);
                    cell.CellStyle = dataStyle;
                    switch ((value??string.Empty).GetType().Name.ToLower())
                    {
                        case "int32":
                        case "int64":
                        case "decimal":
                            dataStyle.Alignment = HorizontalAlignment.RIGHT;
                            cell.SetCellValue(ZConvert.To<double>(value,0));
                            break;
                        default:
                            cell.CellStyle.Alignment = HorizontalAlignment.LEFT;
                            cell.SetCellValue(ZConvert.ToString(value));
                            break;
                    }
                });
            });

            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            sheet = null;
            row = null;

            return ms;
        }

        private static ICellStyle GetHeadStyle(HSSFWorkbook workbook)
        {
            //表头样式
            var headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.LEFT;//居中对齐
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

        private static ICellStyle GetDataStyle(HSSFWorkbook workbook)
        {
            //数据样式
            var dataStyle = workbook.CreateCellStyle();
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

            return dataStyle;
        }
    }
}