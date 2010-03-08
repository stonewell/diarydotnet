using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Program.InitializeDB(txtDatasource.Text.Trim(),
                    txtUsername.Text.Trim(),
                    txtPassword.Text);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
System.Console.Error.WriteLine(ex.StackTrace);
System.Console.Error.WriteLine(ex.InnerException);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (txtDatasource.Text.Trim().Length > 0)
                openFileDialog1.FileName = txtDatasource.Text.Trim();

            if (DialogResult.OK == openFileDialog1.ShowDialog(this))
            {
                txtDatasource.Text = openFileDialog1.FileName;
            }
        }
    }
}
