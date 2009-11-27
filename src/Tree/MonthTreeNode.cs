using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Diary.Net.DB;

namespace Diary.Net.Tree
{
    public class MonthTreeNode : TreeNode
    {
        private int year_ = DateTime.Now.Year;
        private int month_ = DateTime.Now.Month;
        private DayTreeNode[] dayNodes_ = null;

        public MonthTreeNode(int year, int month)
        {
            year_ = year;
            month_ = month;

            Text = month + "ÔÂ";

            dayNodes_ = new DayTreeNode[DateTime.DaysInMonth(year_, month_)];

            for (int i = 0; i < dayNodes_.Length; i++)
                dayNodes_[i] = null;
        }

        public int Year
        {
            get
            {
                return year_;
            }
        }

        public int Month
        {
            get
            {
                return month_;
            }
        }

        public DayTreeNode GetDayNode(int day)
        {
            if (day >= 1 && day <= DateTime.DaysInMonth(year_, month_))
            {
                DayTreeNode node = dayNodes_[day - 1];

                if (node != null)
                    AddToTree(node);

                return node;
            }

            throw new Exception("Invalid Day:" + year_ + "-" + month_ + "-" + day);
        }

        private void AddToTree(DayTreeNode node)
        {
            if (Nodes.Contains(node) || node == null)
                return;

            bool bInserted = false;

            foreach (DayTreeNode dNode in Nodes)
            {
                if (dNode.Day > node.Day)
                {
                    bInserted = true;
                    Nodes.Insert(dNode.Index, node);
                    break;
                }
            }

            if (!bInserted)
            {
                Nodes.Add(node);
            }

            dayNodes_[node.Day - 1] = node;

            node.UpdateText();
            node.Reset();
        }

        public DayTreeNode AddDayNode(int day, int id)
        {
            if (day >= 1 && day <= DateTime.DaysInMonth(year_, month_))
            {
                DayTreeNode node = null;

                if (dayNodes_[day - 1] != null)
                {
                    return dayNodes_[day - 1];
                }
                else
                {
                    node = new DayTreeNode(year_, month_, day, id);
                }

                AddToTree(node);

                return node;
            }

            throw new Exception("Invalid Day:" + year_ + "-" + month_ + "-" + day);
        }
    }
}
