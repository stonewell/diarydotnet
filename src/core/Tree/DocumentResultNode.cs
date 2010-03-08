using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net.Tree
{
    class DocumentResultNode : TreeNode
    {
        public string Title = null;
        public int ID = -1;

        public DocumentResultNode(string title, int id)
        {
            Title = title;
            ID = id;

            Text = title;
        }
    }
}
