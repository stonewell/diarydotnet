using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using HtmlCleaner;

namespace Diary.Net.Html2Rtf
{
    class Resolver : XmlResolver
    {
        public override System.Net.ICredentials Credentials
        {
            set { }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return "";
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return base.ResolveUri(baseUri, relativeUri);
        }
    }

    public static class Html2Rtf
    {
        private static readonly XslCompiledTransform xslt = null;

        static Html2Rtf()
        {
            Assembly ass = typeof(Html2Rtf).Assembly;

            Stream s = null;

            foreach (string name in ass.GetManifestResourceNames())
            {
                if (name.EndsWith("xhtml2rtf.xsl"))
                {
                    s = ass.GetManifestResourceStream(name);
                    break;
                }
            }

            xslt =
                new XslCompiledTransform();

            XsltSettings settings = new XsltSettings();
            settings.EnableDocumentFunction = true;
            settings.EnableScript = true;

            xslt.Load(new XmlTextReader(s), settings, null);
        }

        public static object Transform(string html)
        {
            StringWriter sw = new StringWriter();

            html = GetHtml(html.ToLower());

            xslt.Transform(new XmlTextReader(new StringReader(html)),
                null,
                sw);

            return sw.ToString();
        }

        private static string GetHtml(string html)
        {
            using (StringReader inputFileStream = new StringReader(html))
            {
                HtmlReader reader = new HtmlReader(inputFileStream);

                using (StringWriter outputFileStream = new StringWriter())
                {
                    HtmlWriter writer = new HtmlWriter(outputFileStream);
                    writer.FilterOutput = false;

                    reader.Read();
                    while (!reader.EOF)
                    {
                        writer.WriteNode(reader, true);
                    }

                    return outputFileStream.ToString();
                }
            }
        }

        public static object Clipboard2Rtf()
        {
            using (WebBrowser webBrowser1 = new WebBrowser())
            {

                // Load the MSHTML component
                webBrowser1.Navigate("about:blank");

                // Release control to the system
                Application.DoEvents();

                // Turn ON Design Mode
                ((mshtml.HTMLDocument)webBrowser1.Document.DomDocument).designMode = "On";

                Application.DoEvents();

                // Paste the clipboard contents into the control
                object o = System.Reflection.Missing.Value;

                ((SHDocVw.WebBrowser)webBrowser1.ActiveXInstance).ExecWB(
                   SHDocVw.OLECMDID.OLECMDID_PASTE,
                   SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
                Application.DoEvents();

                // Extract the resulting HTML
                string html = webBrowser1.Document.Body.InnerHtml;

                html = ToXml(webBrowser1.Document.Body);

                html = "<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:xhtml2rtf=\"http://www.lutecia.info/download/xmlns/xhtml2rtf\">" +
                    html + "</html>";

                File.AppendAllText("d:\\1.html", html);
                return Transform(html);
            }
        }

        private static string ToXml(HtmlElement htmlElement)
        {
            if (htmlElement.TagName.Equals("!") ||
                htmlElement.TagName.ToUpper().Equals("SCRIPT"))
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            if (htmlElement.CanHaveChildren && htmlElement.Children.Count > 0)
            {
                sb.Append("<");
                sb.Append(htmlElement.TagName);
                sb.Append(">");
                foreach (HtmlElement child in htmlElement.Children)
                    sb.Append(ToXml(child));
                sb.Append("</");
                sb.Append(htmlElement.TagName);
                sb.Append(">");
            }
            else
            {
                if (htmlElement.InnerHtml == null)
                {
                    sb.Append(htmlElement.OuterHtml);
                    sb.Append("</").Append(htmlElement.TagName).Append(">");
                }
                else
                {
                    sb.Append(htmlElement.OuterHtml);
                }
            }

            return sb.ToString();
        }
    }
}
