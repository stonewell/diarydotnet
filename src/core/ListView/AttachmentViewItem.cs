using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Diary.Net.DB;

namespace Diary.Net.ListView
{
    class AttachmentViewItem : ListViewItem
    {
        private DiaryNetDS.AttachmentsRow row_ = null;
        private string fileName_ = null;

        public AttachmentViewItem(DiaryNetDS.AttachmentsRow row)
        {
            row_ = row;

            if (row_ != null)
            {
                Text = System.IO.Path.GetFileName(row.FileName);
                fileName_ = row_.FileName;
            }
        }

        public DiaryNetDS.AttachmentsRow Row
        {
            get
            {
                return row_;
            }
        }

        public string FileName
        {
            get
            {
                return fileName_;
            }

            set
            {
                fileName_ = value;
                Text = System.IO.Path.GetFileName(fileName_);
            }
        }
    }
}
