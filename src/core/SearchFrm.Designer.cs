namespace Diary.Net
{
    partial class SearchFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFindWhat = new System.Windows.Forms.TextBox();
            this.chkCreateDate = new System.Windows.Forms.CheckBox();
            this.dtpCreateFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpCreateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpModifyTo = new System.Windows.Forms.DateTimePicker();
            this.dtpModifyFrom = new System.Windows.Forms.DateTimePicker();
            this.chkModifyDate = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 38);
            this.panel1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(55, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(162, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.dtpModifyTo);
            this.panel2.Controls.Add(this.dtpModifyFrom);
            this.panel2.Controls.Add(this.chkModifyDate);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpCreateTo);
            this.panel2.Controls.Add(this.dtpCreateFrom);
            this.panel2.Controls.Add(this.chkCreateDate);
            this.panel2.Controls.Add(this.txtFindWhat);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 186);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find What:";
            // 
            // txtFindWhat
            // 
            this.txtFindWhat.Location = new System.Drawing.Point(78, 9);
            this.txtFindWhat.Name = "txtFindWhat";
            this.txtFindWhat.Size = new System.Drawing.Size(202, 20);
            this.txtFindWhat.TabIndex = 0;
            // 
            // chkCreateDate
            // 
            this.chkCreateDate.AutoSize = true;
            this.chkCreateDate.Location = new System.Drawing.Point(13, 36);
            this.chkCreateDate.Name = "chkCreateDate";
            this.chkCreateDate.Size = new System.Drawing.Size(83, 17);
            this.chkCreateDate.TabIndex = 2;
            this.chkCreateDate.Text = "Create Date";
            this.chkCreateDate.UseVisualStyleBackColor = true;
            this.chkCreateDate.CheckedChanged += new System.EventHandler(this.chkCreateDate_CheckedChanged);
            // 
            // dtpCreateFrom
            // 
            this.dtpCreateFrom.Location = new System.Drawing.Point(78, 59);
            this.dtpCreateFrom.Name = "dtpCreateFrom";
            this.dtpCreateFrom.Size = new System.Drawing.Size(202, 20);
            this.dtpCreateFrom.TabIndex = 1;
            // 
            // dtpCreateTo
            // 
            this.dtpCreateTo.Location = new System.Drawing.Point(78, 82);
            this.dtpCreateTo.Name = "dtpCreateTo";
            this.dtpCreateTo.Size = new System.Drawing.Size(202, 20);
            this.dtpCreateTo.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "To:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "To:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "From:";
            // 
            // dtpModifyTo
            // 
            this.dtpModifyTo.Location = new System.Drawing.Point(78, 155);
            this.dtpModifyTo.Name = "dtpModifyTo";
            this.dtpModifyTo.Size = new System.Drawing.Size(202, 20);
            this.dtpModifyTo.TabIndex = 4;
            // 
            // dtpModifyFrom
            // 
            this.dtpModifyFrom.Location = new System.Drawing.Point(78, 132);
            this.dtpModifyFrom.Name = "dtpModifyFrom";
            this.dtpModifyFrom.Size = new System.Drawing.Size(202, 20);
            this.dtpModifyFrom.TabIndex = 3;
            // 
            // chkModifyDate
            // 
            this.chkModifyDate.AutoSize = true;
            this.chkModifyDate.Location = new System.Drawing.Point(13, 109);
            this.chkModifyDate.Name = "chkModifyDate";
            this.chkModifyDate.Size = new System.Drawing.Size(83, 17);
            this.chkModifyDate.TabIndex = 7;
            this.chkModifyDate.Text = "Modify Date";
            this.chkModifyDate.UseVisualStyleBackColor = true;
            this.chkModifyDate.CheckedChanged += new System.EventHandler(this.chkModifyDate_CheckedChanged);
            // 
            // SearchFrm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 224);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchFrm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.Load += new System.EventHandler(this.SearchFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpCreateTo;
        private System.Windows.Forms.DateTimePicker dtpCreateFrom;
        private System.Windows.Forms.CheckBox chkCreateDate;
        private System.Windows.Forms.TextBox txtFindWhat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpModifyTo;
        private System.Windows.Forms.DateTimePicker dtpModifyFrom;
        private System.Windows.Forms.CheckBox chkModifyDate;
    }
}