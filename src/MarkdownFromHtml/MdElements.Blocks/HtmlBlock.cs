using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    class HtmlBlock : IMdBlock
    {
        public string Html { get; }

        public HtmlBlock(string html)
        {
            Html = html;
        }

        public IEnumerable<string> ToMarkdown() => Html.Split('\n');
    }
}
