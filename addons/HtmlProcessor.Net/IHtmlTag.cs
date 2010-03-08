using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlProcessor
{
    public interface IHtmlTag
    {
        string TagName { get; set; }
        IList<IHtmlTagAttribute> Attributes { get; set; }
    }
}
