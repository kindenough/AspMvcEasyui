/*************************************************************************
 * 文件名称 ：PdfExport.cs                          
 * 描述说明 ：导出PDF
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Zephyr.Utils;
using Zephyr.Utils.Gios.Pdf;

namespace Zephyr.Core
{
    //广州-css-泡泡龙(348398942)  17:58:30
    //BaseFont basefont = BaseFont.createFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    //你要引入 这个c:\windows\fonts\simsun.ttc字体
    //pdf不会自动给你引入的

    public class PdfExport:IExport
    {
        public string suffix { get {return "pdf"; } }

        private DataTable table;
        private List<string> title;

        public void Init(object data)
        {
            
            var type = ZGeneric.GetGenericType(data);
            var tableName = ZGeneric.IsDynamicType(type) ? string.Empty : type.Name;
            
            table = new DataTable(tableName);
            EachHelper.EachListHeader(data, (rowIndex, name, cellType) =>
            {
                string typeName = cellType.ToString();
                if (cellType.IsGenericType)
                    typeName = cellType.GetGenericArguments()[0].ToString();

                Type newType = Type.GetType(typeName, false);
                if (newType != null)
                    table.Columns.Add(name, newType);
            });
            table.BeginLoadData();
            title = new List<string>();
        }

        public void MergeCell(int x1,int y1,int x2,int y2)
        {
            throw new Exception("pdf未实现多选title");
        }

        public virtual void FillData(int x, int y,string field, object value)
        {
            if (field.StartsWith("title_"))
            {
                title.Add(field.Split('_')[1]);
                return;
            }

            if (table.Rows.Count< y)
                table.Rows.Add(table.NewRow());
  
            if (value != null && (Type.GetType(value.GetType().ToString(), false) != null))
                table.Rows[y-1][field] = value;
        }

        public virtual void SetHeadStyle(int x1, int y1, int x2, int y2)
        {
           
        }

        public virtual void SetRowsStyle(int x1, int y1, int x2, int y2)
        {
            
        }

        public Stream SaveAsStream()
        {
            table.EndLoadData();
            table.AcceptChanges();

            var removes = new List<string>();
            foreach (DataColumn dc in table.Columns)
                if (title.IndexOf(dc.ColumnName) == -1)
                    removes.Add(dc.ColumnName);

            foreach (var name in removes)
                table.Columns.Remove(name);
 
            var pdfTitle = table.TableName;

            // Starting instantiate the document.
            // Remember to set the Docuement Format. In this case, we specify width and height.
            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 29.7));

            // Now we create a Table of 100 lines, 6 columns and 4 points of Padding.
            PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Arial", 12), table.Rows.Count, table.Columns.Count, 4);

            // Importing datas from the datatables... (also column names for the headers!)
            //myPdfTable.ImportDataTable(Table);
            myPdfTable.ImportDataTable(table);

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
    }
}
