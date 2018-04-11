//create by liuhuisheng 2012-10-27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Data;

namespace Zephyr.Utils.Gios.Pdf
{
    public class PdfHelper
    {
        public static Stream ListToPdf(object list, Dictionary<string, string> titles, bool IsExportAllCol)
        {
            DataTable dt = ListToDataTable(list, titles, IsExportAllCol,string.Empty);
            var pdfTitle = dt.TableName;

            // Starting instantiate the document.
            // Remember to set the Docuement Format. In this case, we specify width and height.
            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 29.7));
            
            // Now we create a Table of 100 lines, 6 columns and 4 points of Padding.
            PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Arial", 12), dt.Rows.Count, dt.Columns.Count, 4);
           
            // Importing datas from the datatables... (also column names for the headers!)
            //myPdfTable.ImportDataTable(Table);
            myPdfTable.ImportDataTable(dt);

            // Sets the format for correct date-time representation
            //myPdfTable.Columns[2].SetContentFormat("{0:dd/MM/yyyy}");

            // Now we set our Graphic Design: Colors and Borders...
            myPdfTable.HeadersRow.SetColors(Color.White, Color.Navy);
            myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
            myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

            //// With just one method we can set the proportional width of the columns.
            //// It's a "percentage like" assignment, but the sum can be different from 100.
            //myPdfTable.SetColumnsWidth(new int[] { 5, 25, 16, 20, 20, 15 });

            //// You can also set colors for a range of cells, in this case, a row:
            //myPdfTable.Rows[7].SetColors(Color.Black, Color.LightGreen);

            // Now we set some alignment... for the whole table and then, for a column.
            myPdfTable.SetContentAlignment(ContentAlignment.MiddleCenter);
            myPdfTable.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);

            // Here we start the loop to generate the table...
            while (!myPdfTable.AllTablePagesCreated)
            {
                // we create a new page to put the generation of the new TablePage:
                PdfPage newPdfPage = myPdfDocument.NewPage();
                PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 48, 120, 500, 670));

                // we also put a Label 
                PdfTextArea pta = new PdfTextArea(new Font("Arial", 26, FontStyle.Bold), Color.Red
                    , new PdfArea(myPdfDocument, 0, 20, 595, 120), ContentAlignment.MiddleCenter, pdfTitle);

                // nice thing: we can put all the objects in the following lines, so we can have
                // a great control of layer sequence... 
                newPdfPage.Add(newPdfTablePage);
                newPdfPage.Add(pta);

                // we save each generated page before start rendering the next.
                newPdfPage.SaveToDocument();
            }
           

            //myPdfDocument.SaveToFile("Example1.pdf");
            var stream = new MemoryStream();
            myPdfDocument.SaveToStream(stream);
            return stream;
        }

        public static DataTable ListToDataTable(object list, Dictionary<string, string> titles, bool IsExportAllCol, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                var type = ZGeneric.GetGenericType(list);
                tableName = ZGeneric.IsDynamicType(type) ? string.Empty : type.Name;
            }

            DataTable table = new DataTable(tableName);
            table.BeginLoadData();
            EachHelper.EachListHeader(list, (rowIndex, name, cellType) =>
            {
                if (IsExportAllCol || titles.ContainsKey(name))
                {
                    string typeName = cellType.ToString();
                    if (cellType.IsGenericType)
                        typeName = cellType.GetGenericArguments()[0].ToString();

                    Type newType = Type.GetType(typeName, false);
                    if (newType != null)
                    {
                        //table.Columns.Add((titles[name] ?? name).ToString(), newType);
                        table.Columns.Add(name, newType);
                    }
                }
            });

            EachHelper.EachListRow(list, (index, rowData) =>
            {
                DataRow row = table.NewRow();
                EachHelper.EachObjectProperty(rowData, (cellIndex, name, value) =>
                {
                    if (IsExportAllCol || titles.ContainsKey(name))
                    {
                        if (value != null && (Type.GetType(value.GetType().ToString(), false) != null))
                            //row[(titles[name] ?? name).ToString()] = value;
                            row[name] = value;
                    }
                });
                table.Rows.Add(row);
            });
            table.EndLoadData();
            table.AcceptChanges();
            return table;
        }
    }
}
