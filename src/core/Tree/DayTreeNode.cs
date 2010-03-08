using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Diary.Net.Tree
{
    public class DayTreeNode : TreeNode
    {
        private int year_ = DateTime.Now.Year;
        private int month_ = DateTime.Now.Month;
        private int day_ = DateTime.Now.Day;
        private int id_ = -1;

        private int commited_id_ = Int32.MinValue;

        public DayTreeNode(int year, int month, int day, int id)
        {
            year_ = year;
            month_ = month;
            day_ = day;
            id_ = id;

            UpdateText();
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

        public int Day
        {
            get
            {
                return day_;
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
            DateTime dt = new DateTime(year_, month_, day_);

            Text = day_ + "»’," + dt.DayOfWeek;
            ForeColor = Color.Black;
        }

        public void Commit()
        {
            commited_id_ = id_;

            UpdateText();
        }

        public void Rollback()
        {
            if (commited_id_ == Int32.MinValue)
            {
                Remove();
            }
            else
            {
                id_ = commited_id_;

                UpdateText();
            }
        }

        public bool Modified
        {
            get
            {
                return id_ != commited_id_;
            }
        }

        public void Reset()
        {
            commited_id_ = Int32.MinValue;
        }
    }
}
