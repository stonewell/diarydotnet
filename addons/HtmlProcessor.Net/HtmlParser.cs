using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HtmlProcessor
{
    public class HtmlParser
    {
        public virtual IHtmlErrorHandler ErrorHandler { get; set; }
        public virtual IHtmlTagHandler TagHandler { get; set; }
        public virtual IList<IHtmlTag> Tags { get; protected set; }

        public virtual void Parse(Stream input)
        {
            Parse(input, Encoding.Default);
        }

        public virtual void Parse(Stream input, Encoding encoding)
        {
            using (StreamReader reader = new StreamReader(input, encoding, true))
            {
            }
        }
    }
}
