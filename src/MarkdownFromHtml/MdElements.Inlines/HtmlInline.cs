using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Inlines
{
    public class HtmlInline : IMdInline
    {
        public string Html { private set; get; }

        public HtmlInline(string html)
        {
            Html = html;
        }

        public string ToMarkdown() => Html.Replace('\n', ' ');

        public void TrimEnd()
        {
            Html = Html.TrimEnd();
        }

        public void TrimStart()
        {
            Html = Html.TrimStart();
        }
    }
}
