using MarkdownFromHtml.MdElements;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    /// <summary>
    /// remove comment element
    /// </summary>
    public class CommentParsre : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { HtmlNode.HtmlNodeTypeNameComment };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();
            return true;
        }
    }
}
