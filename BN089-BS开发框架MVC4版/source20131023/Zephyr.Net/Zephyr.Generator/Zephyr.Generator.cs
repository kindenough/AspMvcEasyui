using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Zephyr.Data;
using System.Configuration;
using Zephyr.Core.Generator;

namespace Zephyr.Generator
{
    public partial class Form1 : Form
    {
        private string RootNodeTag = "root";
        private string RootNodeText = "数据库";
 
        List<string> providers;
        List<string> connections;
        string lastProvider;
        string lastConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var config = new IniFile(".\\Zephyr.Generator.ini");
            providers = config.ReadValue("Settings", "ProviderItems").Trim('|').Split(new string[] { "||" }, StringSplitOptions.None).ToList();
            connections = config.ReadValue("Settings", "ConnectionItems").Trim('|').Split(new string[] { "||" }, StringSplitOptions.None).ToList();
            lastProvider = config.ReadValue("Settings", "lastProvider");
            lastConnection = config.ReadValue("Settings", "lastConnection");

            foreach (var item in providers)
                if (!dbType.Items.Contains(item)) dbType.Items.Add(item);

            ConString.Items.Clear();
            foreach (var item in connections)
                ConString.Items.Add(item);

            dbType.Text = lastProvider;
            ConString.Text = lastConnection;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var config = new IniFile(".\\Zephyr.Generator.ini");
            config.Write("Settings", "ProviderItems",string.Join("||", providers.Distinct().ToArray()));
            config.Write("Settings", "ConnectionItems",string.Join("||", connections.Distinct().ToArray()));
            config.Write("Settings", "lastProvider",lastProvider);
            config.Write("Settings", "lastConnection",lastConnection);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            lastProvider = dbType.Text.Trim();
            lastConnection = ConString.Text.Trim();

            if (providers.IndexOf(lastProvider) <0)
                providers.Add(lastProvider);

            if (connections.IndexOf(lastConnection) < 0)
                connections.Add(lastConnection);
 
            var Tables = GenTables.GetTables(dbType.Text, ConString.Text);

            var ParentNode = new TreeNode {Text = RootNodeText, Tag = RootNodeTag};
            foreach (var T in Tables)
            {
                var TN = new TreeNode {Text = T.TableName, Tag = T, ForeColor = Color.Blue};
                foreach (var TS in T.TableSchemas)
                {
                    var TN2 = new TreeNode {Text = string.Format("{0}[{1}]", TS.ColumnName, TS.SqlTypeName), Tag = TS};
                    TN.Nodes.Add(TN2);
                }
                ParentNode.Nodes.Add(TN);
            }

            this.TableTreeView.Nodes.Clear();
            this.TableTreeView.Nodes.Add(ParentNode);
            this.TableTreeView.Nodes[0].Expand();
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            if (this.TableTreeView.SelectedNode == null || this.TableTreeView.SelectedNode.Tag == null || TableTreeView.SelectedNode.Tag.Equals(RootNodeTag))
            {
                MessageBox.Show("请选择当前需要生成的表", "提示");
                return;
            }

            var CurrentTable = TableTreeView.SelectedNode.Tag as Table;
            var gen = new Generator(CurrentTable){BillName = txtBillName.Text,FileName = txtFileName.Text,ModuleName = txtModule.Text};

            foreach (TreeNode node in TableTreeView.Nodes)
            {
                var T = node.Tag as Table;
                if (T != null && T.TableName == txtDetailTable.Text.Trim())
                    gen.DetailTable = T;
            }

            if (gen.DetailTable == null) gen.DetailTable = CurrentTable;

            this.ModelEdit.Text = gen.GenModel();
            this.DalEdit.Text = gen.GenDAL();
            this.BllEdit.Text = gen.GenBLL();

            this.WebListEdit.Text = gen.GenListAspx();
            this.WebListJsEdit.Text = gen.GenListJs();

            this.txtEditAspx.Text = gen.GenEditAspx();
            this.txtEditJs.Text = gen.GenEditJs();
        }

       
        private void btnAll_Click(object sender, EventArgs e)
        {
            if (TableTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("请先连接数据库", "提示");
                return;
            }

            const string sPath = ".\\Zephyr.Generator.Code";
            const string sPathModel = sPath + "\\model\\";
            const string sPathDAL = sPath + "\\DAL\\";
            const string sPathBLL = sPath + "\\BLL\\";

            deleteFilesAndCreateDir(sPathModel);
            deleteFilesAndCreateDir(sPathDAL);
            deleteFilesAndCreateDir(sPathBLL);
            
            foreach (TreeNode tn in TableTreeView.Nodes[0].Nodes)
            {
                var CurrentTable = tn.Tag as Table;
                var gen = new Generator(CurrentTable);
                var sModel = gen.GenModel();
                var sDAL = gen.GenDAL();
                var sBLL = gen.GenBLL();

                if (CurrentTable == null || tn.Tag.Equals(RootNodeTag)) continue;
                //输出到文件中 model
                using (TextWriter tw = new StreamWriter(new BufferedStream(new FileStream(sPathModel + CurrentTable.TableName + ".cs", FileMode.Create, FileAccess.Write)), System.Text.Encoding.GetEncoding("gbk")))
                {
                    tw.Write(sModel);
                }

                //输出到文件中 dal
                using (TextWriter tw = new StreamWriter(new BufferedStream(new FileStream(sPathDAL +"DAL"+ CurrentTable.TableName + ".cs", FileMode.Create, FileAccess.Write)), System.Text.Encoding.GetEncoding("gbk")))
                {
                    tw.Write(sDAL);
                }

                //输出到文件中 bll
                using (TextWriter tw = new StreamWriter(new BufferedStream(new FileStream(sPathBLL +"BLL"+ CurrentTable.TableName + ".cs", FileMode.Create, FileAccess.Write)), System.Text.Encoding.GetEncoding("gbk")))
                {
                    tw.Write(sBLL);
                }
            }
            System.Diagnostics.Process.Start(sPath); //打开文件夹
        }

        private static void deleteFilesAndCreateDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }

      

       

    }


    public class IniFile
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        private string sPath = null;
        public IniFile(string path)
        {
            this.sPath = path;
        }
        public void Write(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, sPath);
        }
        public string ReadValue(string section, string key)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, sPath);
            return temp.ToString();
        }
    }
}