using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Diary.Net.Tree
{
    class DocumentTreeNode : TreeNode
    {
        private int id_ = -1;
        private string title_ = null;

        private string commitedTitle_ = null;
        private int commitedId_ = Int32.MinValue;

        public string Title
        {
            get
            {
                return title_;
            }

            set
            {
                title_ = value;

                UpdateText();
            }
        }

        public int ID
        {
            get
            {
                return id_;
            }

            set
            {
                id_ = value;

                UpdateText();
            }
        }

        public void UpdateText()
        {
             Text = title_;
        }

        public void Rollback()
        {
            Title = commitedTitle_;
            ID = commitedId_;

            UpdateText();

            if (commitedId_ == Int32.MinValue)
                Remove();
        }

        public void Commit()
        {
            commitedId_ = ID;
            commitedTitle_ = Title;

            UpdateText();
        }

        public bool Modified
        {
            get
            {
                return title_ != commitedTitle_ ||
                    id_ != commitedId_;
            }
        }
    }
}
