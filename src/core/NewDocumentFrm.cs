using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net
{
    public partial class NewDocumentFrm : Form
    {
        private string title_ = null;

        public NewDocumentFrm()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return title_;
            }

            set
            {
                title_ = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0)
            {
                label1.ForeColor = Color.Red;

                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }

            title_ = txtTitle.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void NewDocumentFrm_Load(object sender, EventArgs e)
        {
            if (title_ != null)
                txtTitle.Text = title_;
        }
    }
}