using System.Windows.Forms;

namespace L2Robot
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonIGStart = new System.Windows.Forms.Button();
            this.listView_instances = new System.Windows.Forms.ListView();
            this.ch1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonOOG = new System.Windows.Forms.Button();
            this.textBoxScriptFile = new System.Windows.Forms.TextBox();
            this.buttonRunScript = new System.Windows.Forms.Button();
            this.buttonLoadScript = new System.Windows.Forms.Button();
            this.openFileDialogScript = new System.Windows.Forms.OpenFileDialog();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonIGStart
            // 
            this.buttonIGStart.Location = new System.Drawing.Point(28, 23);
            this.buttonIGStart.Name = "buttonIGStart";
            this.buttonIGStart.Size = new System.Drawing.Size(101, 32);
            this.buttonIGStart.TabIndex = 0;
            this.buttonIGStart.Text = "开始监听";
            this.buttonIGStart.UseVisualStyleBackColor = true;
            this.buttonIGStart.Click += new System.EventHandler(this.buttonIGStart_Click);
            // 
            // listView_instances
            // 
            this.listView_instances.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch1,
            this.ch2,
            this.ch3});
            this.listView_instances.FullRowSelect = true;
            this.listView_instances.HideSelection = false;
            this.listView_instances.Location = new System.Drawing.Point(28, 77);
            this.listView_instances.MultiSelect = false;
            this.listView_instances.Name = "listView_instances";
            this.listView_instances.Size = new System.Drawing.Size(371, 211);
            this.listView_instances.TabIndex = 1;
            this.listView_instances.UseCompatibleStateImageBehavior = false;
            this.listView_instances.View = System.Windows.Forms.View.Details;
            this.listView_instances.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_instances_ItemSelectionChanged);
            // 
            // ch1
            // 
            this.ch1.Text = "人物";
            this.ch1.Width = 120;
            // 
            // ch2
            // 
            this.ch2.Text = "客户端口";
            this.ch2.Width = 120;
            // 
            // ch3
            // 
            this.ch3.Text = "状态";
            this.ch3.Width = 120;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.buttonOOG);
            this.panel1.Controls.Add(this.textBoxScriptFile);
            this.panel1.Controls.Add(this.buttonRunScript);
            this.panel1.Controls.Add(this.buttonLoadScript);
            this.panel1.Location = new System.Drawing.Point(460, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 206);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 24);
            this.button1.TabIndex = 10;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonOOG
            // 
            this.buttonOOG.Location = new System.Drawing.Point(15, 109);
            this.buttonOOG.Name = "buttonOOG";
            this.buttonOOG.Size = new System.Drawing.Size(72, 24);
            this.buttonOOG.TabIndex = 9;
            this.buttonOOG.Text = "设置脱机";
            this.buttonOOG.UseVisualStyleBackColor = true;
            this.buttonOOG.Visible = false;
            // 
            // textBoxScriptFile
            // 
            this.textBoxScriptFile.Enabled = false;
            this.textBoxScriptFile.Location = new System.Drawing.Point(109, 17);
            this.textBoxScriptFile.Name = "textBoxScriptFile";
            this.textBoxScriptFile.Size = new System.Drawing.Size(194, 21);
            this.textBoxScriptFile.TabIndex = 8;
            // 
            // buttonRunScript
            // 
            this.buttonRunScript.Enabled = false;
            this.buttonRunScript.Location = new System.Drawing.Point(15, 62);
            this.buttonRunScript.Name = "buttonRunScript";
            this.buttonRunScript.Size = new System.Drawing.Size(73, 24);
            this.buttonRunScript.TabIndex = 7;
            this.buttonRunScript.Text = "点击开始";
            this.buttonRunScript.UseVisualStyleBackColor = true;
            this.buttonRunScript.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // buttonLoadScript
            // 
            this.buttonLoadScript.Location = new System.Drawing.Point(15, 16);
            this.buttonLoadScript.Name = "buttonLoadScript";
            this.buttonLoadScript.Size = new System.Drawing.Size(73, 24);
            this.buttonLoadScript.TabIndex = 6;
            this.buttonLoadScript.Text = "选择脚本";
            this.buttonLoadScript.UseVisualStyleBackColor = true;
            this.buttonLoadScript.Click += new System.EventHandler(this.buttonLoadScript_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(28, 315);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(766, 318);
            this.textBoxLog.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 645);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView_instances);
            this.Controls.Add(this.buttonIGStart);
            this.Name = "FormMain";
            this.Text = "L2R";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonIGStart;
        private ListView listView_instances;
        private ColumnHeader ch1;
        private ColumnHeader ch2;
        private ColumnHeader ch3;
        private Panel panel1;
        private Button buttonLoadScript;
        private Button buttonRunScript;
        private OpenFileDialog openFileDialogScript;
        private TextBox textBoxScriptFile;
        private Button buttonOOG;
        private Button button1;
        private TextBox textBoxLog;
        private ContextMenuStrip contextMenuStrip1;
    }
}

