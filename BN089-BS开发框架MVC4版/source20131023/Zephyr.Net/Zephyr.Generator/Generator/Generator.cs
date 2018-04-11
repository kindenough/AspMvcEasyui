using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Zephyr.Core.Generator;

namespace Zephyr.Generator
{
    public class Generator
    {
        public Table CurrentTable { get; set; }
        public string ModuleName { get; set; }
        public string BillName { get; set; }
        public string FileName { get; set; }
        public Table DetailTable { get; set; }

        const string TemplatePath = @".\Zephyr.Generator.Template\";
        const string sPathModel = TemplatePath + "Model.txt";
        const string sPathDAL = TemplatePath + "DAL.txt";
        const string sPathBLL = TemplatePath + "BLL.txt";
        const string sPathWebList = TemplatePath + "web_list.aspx";
        const string sPathWebEdit = TemplatePath + "web_edit.aspx";

        private static string ToStr(string str, string sDefault = "")
        {
            return string.IsNullOrEmpty(str) ? sDefault : str;
        }

        public Generator(Table table)
        {
            this.CurrentTable = table;
        }

        public string GenModel()
        {
            var TModel = ReadTxtFile(sPathModel);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
        
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8,' ') + "{0}";
            foreach (var TS in CurrentTable.TableSchemas)
            {
                if (TS.IsIdentity) sField += String.Format(sSpaces, "[Identity]");
                if (TS.IsPrimaryKey) sField += String.Format(sSpaces, "[PrimaryKey]");
                sField += Temp.Replace("{FieldType}", TS.TypeName).Replace("{FieldName}", TS.ColumnName);
            }
            TModel = TModel.Replace("{TableName}", CurrentTable.TableName);
            TModel = RegField.Replace(TModel, sField);
            return TModel;
        }

        public string GenDAL()
        {
            var TDAL = ReadTxtFile(sPathDAL);
            TDAL = TDAL.Replace("{TableName}", CurrentTable.TableName);
            return TDAL;
        }

        public string GenBLL()
        {
            var TBLL = ReadTxtFile(sPathBLL);
            TBLL = TBLL.Replace("{TableName}", CurrentTable.TableName);
            //TBLL = TBLL.Replace("IDOnePlusTable", CurrentTable.Identity);
            return TBLL;
        }

        public string GenListAspx()
        {
            var ts = CurrentTable.TableSchemas;
            var TWebList = ReadTxtFile(sPathWebList);
            for (var i = 0; i < ts.Count; i++)
            {
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "Class"), "z-txt" + ts[i].TypeName == "DateTime" ? " easyui-datebox" : "");
            }

            TWebList = TWebList.Replace("{FileName}",String.Format("/{0}/{1}", ToStr(ModuleName),ToStr(FileName,CurrentTable.TableName)).Replace("//",""));
            return TWebList;
        }

        public string GenListJs()
        {
          
            var s = string.Empty;
            const string template = "{ title: '{0}', field: '{0}', align: 'left', width: 70, formatter: function (value) { return $.formatDate(value, 'yyyy-MM-dd'); } },";
            var ts = CurrentTable.TableSchemas;
            var TWebList = ReadTxtFile(sPathWebList + ".js");
            for (var i = 0; i < ts.Count; i++)
            {
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
                
                var c = ts[i];
                s += s.Length == 0 ? "{" : "\t\t{";
                s += String.Format("{0}:'{1}',","title",c.ColumnName);
                s += String.Format("{0}:'{1}',", "field", c.ColumnName);
                s += String.Format("{0}:'{1}',", "align", "left");
                s += String.Format("{0}:{1},", "width", 70);
                if (ts[i].TypeName.StartsWith("DateTime"))
                    s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatDate(value, 'yyyy-MM-dd'); }");
                if (ts[i].TypeName.StartsWith("decimal"))
                    s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatNumber(value, '#,##0.00'); }");
                s = s.Trim(',') + (i == ts.Count - 1 ? "}\r\n\t\t" : "},\r\n");
            }
            TWebList = TWebList.Replace("{cols}", s.Trim(','));
            TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            if (BillName!=null) TWebList = TWebList.Replace("{BillName}", BillName);
            TWebList = TWebList.Replace("{FileName}", String.Format("/{0}/{1}", ToStr(ModuleName), ToStr(FileName, CurrentTable.TableName)).Replace("//", ""));
            return TWebList;
        }

       


        public string GenEditAspx()
        {
            var ts = CurrentTable.TableSchemas;
            var TWebList = ReadTxtFile(sPathWebEdit);
            for (var i = 0; i < ts.Count; i++)
            {
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "Class"), "z-txt" + ts[i].TypeName == "DateTime" ? " easyui-datebox" : "");
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "DataCp"), ts[i].IsPrimaryKey?"data-cp=\"equal\" readonly=\"readonly\" ":"");
            }
            TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            TWebList = TWebList.Replace("{FileName}", String.Format("/{0}/{1}", ToStr(ModuleName), ToStr(FileName, CurrentTable.TableName)).Replace("//", ""));
            return TWebList;
        }

        public string GenEditJs()
        {
            var s = string.Empty;
            const string template = "{ title: '{0}', field: '{0}', align: 'left', width: 70, formatter: function (value) { return $.formatDate(value, 'yyyy-MM-dd'); } },";
            var ts = CurrentTable.TableSchemas;
            var TWebList = ReadTxtFile(sPathWebEdit + ".js");
            for (var i = 0; i < ts.Count; i++)
            {
                TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
            }

            ts = DetailTable.TableSchemas;
            for (var i = 0; i < ts.Count; i++)
            {
                var c = ts[i];
                s += s.Length == 0 ? "{" : "\t\t{";
                s += String.Format("{0}:'{1}',", "title", c.ColumnName);
                s += String.Format("{0}:'{1}',", "field", c.ColumnName);
                s += String.Format("{0}:'{1}',", "align", "left");
                s += String.Format("{0}:{1},", "width", 70);
                if (ts[i].TypeName.StartsWith("DateTime"))
                    s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatDate(value, 'yyyy-MM-dd'); }");
                if (ts[i].TypeName.StartsWith("decimal"))
                    s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatNumber(value, '#,##0.00'); }");
                s = s.Trim(',') + (i == ts.Count - 1 ? "}\r\n\t\t" : "},\r\n");
            }

            TWebList = TWebList.Replace("{DetailCols}", s.Trim(','));
            TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            TWebList = TWebList.Replace("{DetailTableName}", DetailTable.TableName);
      
            return TWebList;
        }

        public string ReadTxtFile(string FilePath)
        {
            var content = string.Empty;//返回的字符串
            using (var fs = new FileStream(FilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    string text = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        text += reader.ReadLine() + "\r\n";
                        content = text;
                    }
                }
            }
            return content;
        }
    }
}
