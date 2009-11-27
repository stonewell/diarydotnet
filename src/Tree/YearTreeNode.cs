using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Diary.Net.DB;

namespace Diary.Net.Tree
{
    public class YearTreeNode : TreeNode
    {
        private int year_ = DateTime.Now.Year;
        private MonthTreeNode[] monthNodes_ = new MonthTreeNode[12];

        public YearTreeNode(int year)
        {
            year_ = year;

            Text = year + "Äê";

            for (int i = 0; i < 12; i++)
                monthNodes_[i] = null;
        }

        public int Year
        {
            get
            {
                return year_;
            }
        }

        public MonthTreeNode GetMonthNode(int month)
        {
            if (month >= 1 && month <= 12)
            {
                return monthNodes_[month - 1];
            }

            throw new Exception("Invalid Month:" + month);
        }

        public MonthTreeNode AddMonthNode(int month)
        {
            if (month >= 1 && month <= 12)
            {
                if (monthNodes_[month - 1] != null)
                    return monthNodes_[month - 1];

                MonthTreeNode node = new MonthTreeNode(year_, month);

                bool bInserted = false;

                foreach (MonthTreeNode mNode in Nodes)
                {
                    if (mNode.Month > month)
                    {
                        bInserted = true;
                        Nodes.Insert(mNode.Index, node);
                        break;
                    }
                }

                if (!bInserted)
                {
                    Nodes.Add(node);
                }

                monthNodes_[month - 1] = node;

                return node;
            }

            throw new Exception("Invalid Month:" + month);
        }
    }
}
