using MarkdownFromHtml.MdElements;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class ParagraphParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "p", "div" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = manager.ParseAndGroup(node.ChildNodes);
            return true;
        }
    }
}
