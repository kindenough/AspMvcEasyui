using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using Zephyr.Utils.EPPlus.Style;
using System.Drawing;

namespace Zephyr.Utils.EPPlus
{
    public class EPPlusHelper
    {
        public static MemoryStream ListToExcel(object list, Dictionary<string, string> titles, bool IsExportAllCol)
        {
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("sheet1");
                var fields = titles.Keys.ToList();
                const int startIndex = 1;
                var currentRow = startIndex;
                var currentCell = 0;

                EachHelper.EachListHeader(list, (i, name, type) => 
                {
                    if (!fields.Contains(name))
                    {
                        if (IsExportAllCol)
                            fields.Add(name);
                        else
                            return;
                    }
                    currentCell = fields.IndexOf(name) + startIndex;
                    sheet.Cells[currentRow, currentCell].Value = titles[name] ?? name;
                    sheet.Column(currentCell).AutoFit();
                });

                EachHelper.EachListRow(list, (rowIndex, rowData) => 
                {
                    currentRow = rowIndex + startIndex + 1;
                    EachHelper.EachObjectProperty(rowData, (i, name, value) =>
                    {
                        if (!fields.Contains(name))
                        {
                            if (IsExportAllCol)
                                fields.Add(name);
                            else
                                return;
                        }
                        currentCell = fields.IndexOf(name) + startIndex;
                        if (ZGeneric.IsTypeIgoreNullable<DateTime>(value))
                            sheet.Column(currentCell).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";

                        sheet.Cells[currentRow, currentCell].Value = value ?? string.Empty;
                    });
                });

                currentCell = startIndex + fields.Count - 1;
                using (var head = sheet.Cells[startIndex, startIndex, startIndex, currentCell]) // set head style
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

                using (var data = sheet.Cells[startIndex + 1, startIndex, currentRow, currentCell])// set data style
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

                var ms = new MemoryStream();
                package.SaveAs(ms);
                return ms;
            }
        }

        public static MemoryStream CreateByExcelLibrary(DataTable table)
        {
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("sheet111");

                var colCount = table.Columns.Count;
                for (var i = 0; i < colCount; i++)
                {
                    sheet.Cells[1, i + 1].Value = table.Columns[i].Caption;
                }

                var k = 2;
                foreach (DataRow row in table.Rows)
                {
                    for (var i = 0; i < colCount; i++)
                    {
                        sheet.Cells[k, i + 1].Value = row[i];
                    }
                    k++;
                }

                var ms = new MemoryStream();
                package.SaveAs(ms);
                return ms;
            }
        }

        public static DataTable ReadByExcelLibrary(Stream xlsStream)
        {
            var table = new DataTable();
            using (var package = new ExcelPackage(xlsStream))
            {
                var sheet = package.Workbook.Worksheets[1];

                var colCount = sheet.Dimension.End.Column;
                var rowCount = sheet.Dimension.End.Row;

                for (ushort j = 1; j <= colCount; j++)
                {
                    table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));
                }

                for (ushort i = 2; i <= rowCount; i++)
                {
                    var row = table.NewRow();
                    for (ushort j = 1; j <= colCount; j++)
                    {
                        row[j - 1] = sheet.Cells[i, j].Value;
                    }
                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}