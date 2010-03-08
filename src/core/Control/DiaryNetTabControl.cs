using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Diary.Net.Control
{
#if __MonoCS__
    public partial class DiaryNetTabControl : TabControl
#else
    public partial class DiaryNetTabControl : FlatTabControl.FlatTabControl
#endif
    {
        public DiaryNetTabControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

#if __MonoCS__
        public Color myBackColor
        {
            get
            {
                return BackColor;
            }

            set
            {
                BackColor = value;
            }
        }
#endif
    }
}
