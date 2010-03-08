using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net
{
    public partial class SelectDateFrm : Form
    {
        private DateTime selectedDateTime_ = DateTime.Now;

        public SelectDateFrm()
        {
            InitializeComponent();
        }

        private void SelectDateFrm_Load(object sender, EventArgs e)
        {
            monthCalendar1.SelectionStart = selectedDateTime_;
            monthCalendar1.SelectionEnd = selectedDateTime_;
        }

        public DateTime SelectedDate
        {
            get
            {
                return selectedDateTime_;
            }

            set
            {
                selectedDateTime_ = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedDateTime_ = monthCalendar1.SelectionStart;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}