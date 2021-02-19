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
            this.buttonIGStart = new System.Windows.Forms.Button();
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
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonIGStart);
            this.Name = "FormMain";
            this.Text = "L2R";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonIGStart;
    }
}

