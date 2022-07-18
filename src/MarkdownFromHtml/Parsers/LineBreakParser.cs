using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class LineBreakParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "br" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = new[] { new Linebreak() };
            return true;
        }
    }
}
