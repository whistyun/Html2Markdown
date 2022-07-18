using System.Collections.Generic;

namespace MarkdownFromHtml.MdElements.Blocks
{
    class HtmlBlock : IMdBlock
    {
        public string Html { get; }

        public HtmlBlock(string html)
        {
            Html = html;
        }

        public IEnumerable<string> ToMarkdown() => new[] { Html };
    }
}
