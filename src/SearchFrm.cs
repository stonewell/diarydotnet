using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net
{
    public partial class SearchFrm : Form
    {
        private string findWhat_ = null;

        private bool useCreate_ = false;
        private DateTime createFrom_ = DateTime.Today;
        private DateTime createTo_ = DateTime.Today;

        private bool useModify_ = false;
        private DateTime modifyFrom_ = DateTime.Today;
        private DateTime modifyTo_ = DateTime.Today;

        public SearchFrm()
        {
            InitializeComponent();
        }

        public string FindWhat
        {
            get
            {
                return findWhat_;
            }
        }

        public bool UseCreate
        {
            get
            {
                return useCreate_;
            }
        }

        public DateTime CreateFrom
        {
            get
            {
                return createFrom_;
            }
        }

        public DateTime CreateTo
        {
            get
            {
                return createTo_;
            }
        }

        public bool UseModify
        {
            get
            {
                return useModify_;
            }
        }

        public DateTime ModifyFrom
        {
            get
            {
                return modifyFrom_;
            }
        }

        public DateTime ModifyTo
        {
            get
            {
                return modifyTo_;
            }
        }

        private void SearchFrm_Load(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            dtpCreateFrom.Enabled = chkCreateDate.Checked;
            dtpCreateTo.Enabled = chkCreateDate.Checked;

            dtpModifyFrom.Enabled = chkModifyDate.Checked;
            dtpModifyTo.Enabled = chkModifyDate.Checked;
        }

        private void chkCreateDate_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void chkModifyDate_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (txtFindWhat.Text.Trim().Length == 0)
            {
                label1.ForeColor = Color.Red;

                return;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }

            findWhat_ = txtFindWhat.Text.Trim();

            useCreate_ = chkCreateDate.Checked;
            createFrom_ = dtpCreateFrom.Value;
            createTo_ = dtpCreateTo.Value;

            useModify_ = chkModifyDate.Checked;
            modifyFrom_ = dtpModifyFrom.Value;
            modifyTo_ = dtpModifyTo.Value;

            Close();
        }
    }
}