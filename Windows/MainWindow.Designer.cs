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
            this.ch4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialogScript = new System.Windows.Forms.OpenFileDialog();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataGridViewNearPlayer = new System.Windows.Forms.DataGridView();
            this.GRPlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonGetHwd = new System.Windows.Forms.Button();
            this.listViewBlackList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearPlayer)).BeginInit();
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
            this.ch3,
            this.ch4,
            this.ch5});
            this.listView_instances.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_instances.FullRowSelect = true;
            this.listView_instances.HideSelection = false;
            this.listView_instances.Location = new System.Drawing.Point(28, 77);
            this.listView_instances.MultiSelect = false;
            this.listView_instances.Name = "listView_instances";
            this.listView_instances.Scrollable = false;
            this.listView_instances.Size = new System.Drawing.Size(564, 211);
            this.listView_instances.TabIndex = 1;
            this.listView_instances.UseCompatibleStateImageBehavior = false;
            this.listView_instances.View = System.Windows.Forms.View.Details;
            this.listView_instances.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView_instances_ColumnWidthChanging);
            this.listView_instances.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_instances_ItemSelectionChanged);
            // 
            // ch1
            // 
            this.ch1.Text = "人物";
            this.ch1.Width = 120;
            // 
            // ch2
            // 
            this.ch2.Text = "进程ID";
            this.ch2.Width = 120;
            // 
            // ch3
            // 
            this.ch3.Text = "位置";
            this.ch3.Width = 169;
            // 
            // ch4
            // 
            this.ch4.Text = "鱼飞状态（点击激活）";
            this.ch4.Width = 138;
            // 
            // ch5
            // 
            this.ch5.Text = "ID";
            this.ch5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ch5.Width = 200;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(28, 315);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(564, 318);
            this.textBoxLog.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dataGridViewNearPlayer
            // 
            this.dataGridViewNearPlayer.AllowUserToAddRows = false;
            this.dataGridViewNearPlayer.AllowUserToDeleteRows = false;
            this.dataGridViewNearPlayer.AllowUserToResizeColumns = false;
            this.dataGridViewNearPlayer.AllowUserToResizeRows = false;
            this.dataGridViewNearPlayer.CausesValidation = false;
            this.dataGridViewNearPlayer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNearPlayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GRPlayerName});
            this.dataGridViewNearPlayer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewNearPlayer.Location = new System.Drawing.Point(609, 315);
            this.dataGridViewNearPlayer.MultiSelect = false;
            this.dataGridViewNearPlayer.Name = "dataGridViewNearPlayer";
            this.dataGridViewNearPlayer.ReadOnly = true;
            this.dataGridViewNearPlayer.RowHeadersVisible = false;
            this.dataGridViewNearPlayer.RowTemplate.Height = 23;
            this.dataGridViewNearPlayer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewNearPlayer.Size = new System.Drawing.Size(210, 318);
            this.dataGridViewNearPlayer.TabIndex = 4;
            this.dataGridViewNearPlayer.TabStop = false;
            // 
            // GRPlayerName
            // 
            this.GRPlayerName.DataPropertyName = "PlayerName";
            this.GRPlayerName.HeaderText = "周围玩家";
            this.GRPlayerName.Name = "GRPlayerName";
            this.GRPlayerName.ReadOnly = true;
            this.GRPlayerName.Width = 200;
            // 
            // buttonGetHwd
            // 
            this.buttonGetHwd.Location = new System.Drawing.Point(160, 23);
            this.buttonGetHwd.Name = "buttonGetHwd";
            this.buttonGetHwd.Size = new System.Drawing.Size(120, 32);
            this.buttonGetHwd.TabIndex = 5;
            this.buttonGetHwd.Text = "点击获取游戏窗口";
            this.buttonGetHwd.UseVisualStyleBackColor = true;
            this.buttonGetHwd.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonGetHwd_MouseClick);
            this.buttonGetHwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonGetHwd_MouseDown);
            this.buttonGetHwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonGetHwd_MouseUp);
            // 
            // listViewBlackList
            // 
            this.listViewBlackList.Enabled = false;
            this.listViewBlackList.HideSelection = false;
            this.listViewBlackList.Location = new System.Drawing.Point(609, 78);
            this.listViewBlackList.Name = "listViewBlackList";
            this.listViewBlackList.Size = new System.Drawing.Size(209, 210);
            this.listViewBlackList.TabIndex = 6;
            this.listViewBlackList.UseCompatibleStateImageBehavior = false;
            this.listViewBlackList.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(612, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "黑名单";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 638);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewBlackList);
            this.Controls.Add(this.buttonGetHwd);
            this.Controls.Add(this.dataGridViewNearPlayer);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.listView_instances);
            this.Controls.Add(this.buttonIGStart);
            this.Name = "FormMain";
            this.Text = "L2R";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearPlayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonIGStart;
        private ColumnHeader ch1;
        private ColumnHeader ch2;
        private ColumnHeader ch3;
        private OpenFileDialog openFileDialogScript;
        private TextBox textBoxLog;
        private ContextMenuStrip contextMenuStrip1;
        private ListView listView_instances;
        private DataGridView dataGridViewNearPlayer;
        private DataGridViewTextBoxColumn GRPlayerName;
        private ColumnHeader ch4;
        private Button buttonGetHwd;
        private ListView listViewBlackList;
        private Label label1;
        private ColumnHeader ch5;
    }
}

