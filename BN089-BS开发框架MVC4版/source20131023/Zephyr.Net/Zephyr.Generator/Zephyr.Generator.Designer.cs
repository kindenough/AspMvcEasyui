namespace Zephyr.Generator
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.TableTreeView = new System.Windows.Forms.TreeView();
            this.btnCode = new System.Windows.Forms.Button();
            this.ModelEdit = new System.Windows.Forms.RichTextBox();
            this.DalEdit = new System.Windows.Forms.RichTextBox();
            this.BllEdit = new System.Windows.Forms.RichTextBox();
            this.WebListEdit = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.WebListJsEdit = new System.Windows.Forms.RichTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtEditAspx = new System.Windows.Forms.RichTextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.txtEditJs = new System.Windows.Forms.RichTextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtDetailTable = new System.Windows.Forms.TextBox();
            this.txtBillName = new System.Windows.Forms.TextBox();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.ConString = new System.Windows.Forms.ComboBox();
            this.dbType = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(191, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "连接数据库";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // TableTreeView
            // 
            this.TableTreeView.Location = new System.Drawing.Point(1, 31);
            this.TableTreeView.Name = "TableTreeView";
            this.TableTreeView.Size = new System.Drawing.Size(182, 475);
            this.TableTreeView.TabIndex = 1;
            // 
            // btnCode
            // 
            this.btnCode.Location = new System.Drawing.Point(279, 31);
            this.btnCode.Name = "btnCode";
            this.btnCode.Size = new System.Drawing.Size(75, 23);
            this.btnCode.TabIndex = 5;
            this.btnCode.Text = "生成代码";
            this.btnCode.UseVisualStyleBackColor = true;
            this.btnCode.Click += new System.EventHandler(this.btnCode_Click);
            // 
            // ModelEdit
            // 
            this.ModelEdit.Location = new System.Drawing.Point(0, 0);
            this.ModelEdit.Name = "ModelEdit";
            this.ModelEdit.Size = new System.Drawing.Size(624, 454);
            this.ModelEdit.TabIndex = 6;
            this.ModelEdit.Text = "Model";
            // 
            // DalEdit
            // 
            this.DalEdit.Location = new System.Drawing.Point(0, 0);
            this.DalEdit.Name = "DalEdit";
            this.DalEdit.Size = new System.Drawing.Size(624, 450);
            this.DalEdit.TabIndex = 7;
            this.DalEdit.Text = "DAL";
            // 
            // BllEdit
            // 
            this.BllEdit.Location = new System.Drawing.Point(0, 3);
            this.BllEdit.Name = "BllEdit";
            this.BllEdit.Size = new System.Drawing.Size(620, 447);
            this.BllEdit.TabIndex = 8;
            this.BllEdit.Text = "BLL";
            // 
            // WebListEdit
            // 
            this.WebListEdit.Location = new System.Drawing.Point(0, 0);
            this.WebListEdit.Name = "WebListEdit";
            this.WebListEdit.Size = new System.Drawing.Size(627, 417);
            this.WebListEdit.TabIndex = 9;
            this.WebListEdit.Text = "//Todo aspx";
            this.WebListEdit.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Location = new System.Drawing.Point(189, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(628, 443);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ModelEdit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(620, 417);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Model";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DalEdit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(620, 417);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DAL";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.BllEdit);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(620, 417);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "BLL";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.WebListEdit);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(620, 417);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "search.aspx";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.WebListJsEdit);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(620, 417);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "search.js";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // WebListJsEdit
            // 
            this.WebListJsEdit.Location = new System.Drawing.Point(0, 3);
            this.WebListJsEdit.Name = "WebListJsEdit";
            this.WebListJsEdit.Size = new System.Drawing.Size(620, 418);
            this.WebListJsEdit.TabIndex = 0;
            this.WebListJsEdit.Text = "//Todo js";
            this.WebListJsEdit.WordWrap = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txtEditAspx);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(620, 417);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "edit.aspx";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // txtEditAspx
            // 
            this.txtEditAspx.Location = new System.Drawing.Point(-3, 0);
            this.txtEditAspx.Name = "txtEditAspx";
            this.txtEditAspx.Size = new System.Drawing.Size(627, 417);
            this.txtEditAspx.TabIndex = 10;
            this.txtEditAspx.Text = "//Todo edit.aspx";
            this.txtEditAspx.WordWrap = false;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.txtEditJs);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(620, 417);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "edit.js";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // txtEditJs
            // 
            this.txtEditJs.Location = new System.Drawing.Point(-3, 0);
            this.txtEditJs.Name = "txtEditJs";
            this.txtEditJs.Size = new System.Drawing.Size(627, 417);
            this.txtEditJs.TabIndex = 10;
            this.txtEditJs.Text = "//Todo edit.js";
            this.txtEditJs.WordWrap = false;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.label4);
            this.tabPage8.Controls.Add(this.label3);
            this.tabPage8.Controls.Add(this.label2);
            this.tabPage8.Controls.Add(this.label1);
            this.tabPage8.Controls.Add(this.txtFileName);
            this.tabPage8.Controls.Add(this.txtDetailTable);
            this.tabPage8.Controls.Add(this.txtBillName);
            this.tabPage8.Controls.Add(this.txtModule);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(620, 417);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "设置";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "文件名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "明细表名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "单据名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "模块名称";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(79, 81);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(216, 21);
            this.txtFileName.TabIndex = 0;
            // 
            // txtDetailTable
            // 
            this.txtDetailTable.Location = new System.Drawing.Point(79, 166);
            this.txtDetailTable.Name = "txtDetailTable";
            this.txtDetailTable.Size = new System.Drawing.Size(216, 21);
            this.txtDetailTable.TabIndex = 0;
            // 
            // txtBillName
            // 
            this.txtBillName.Location = new System.Drawing.Point(79, 125);
            this.txtBillName.Name = "txtBillName";
            this.txtBillName.Size = new System.Drawing.Size(216, 21);
            this.txtBillName.TabIndex = 0;
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(79, 43);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(216, 21);
            this.txtModule.TabIndex = 0;
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(369, 31);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 12;
            this.btnAll.Text = "全部生成";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // ConString
            // 
            this.ConString.FormattingEnabled = true;
            this.ConString.Items.AddRange(new object[] {
            "server=.\\SQL2005;uid=sa;pwd=123456;database=Zephyr.Mms"});
            this.ConString.Location = new System.Drawing.Point(115, 4);
            this.ConString.Name = "ConString";
            this.ConString.Size = new System.Drawing.Size(702, 20);
            this.ConString.TabIndex = 13;
            this.ConString.Text = "server=192.168.1.100\\SQL2005;database=Zephyr.MMS;UID=sa;PWD=123456";
            // 
            // dbType
            // 
            this.dbType.FormattingEnabled = true;
            this.dbType.Items.AddRange(new object[] {
            "SqlServer",
            "Oracle"});
            this.dbType.Location = new System.Drawing.Point(2, 4);
            this.dbType.Name = "dbType";
            this.dbType.Size = new System.Drawing.Size(107, 20);
            this.dbType.TabIndex = 14;
            this.dbType.Text = "SqlServer";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 507);
            this.Controls.Add(this.dbType);
            this.Controls.Add(this.ConString);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCode);
            this.Controls.Add(this.TableTreeView);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zephyr.Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TreeView TableTreeView;
        private System.Windows.Forms.Button btnCode;
        private System.Windows.Forms.RichTextBox ModelEdit;
        private System.Windows.Forms.RichTextBox DalEdit;
        private System.Windows.Forms.RichTextBox BllEdit;
        private System.Windows.Forms.RichTextBox WebListEdit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.RichTextBox WebListJsEdit;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.ComboBox ConString;
        private System.Windows.Forms.ComboBox dbType;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.RichTextBox txtEditAspx;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.RichTextBox txtEditJs;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetailTable;
        private System.Windows.Forms.TextBox txtBillName;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileName;
    }
}

