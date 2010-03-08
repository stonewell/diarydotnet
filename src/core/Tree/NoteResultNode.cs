using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net.Tree
{
    class NoteResultNode : TreeNode
    {
        public DateTime Date = DateTime.Now;

        public NoteResultNode(DateTime dt)
        {
            Date = dt;

            Text = Date.ToLongDateString();
        }
    }
}
