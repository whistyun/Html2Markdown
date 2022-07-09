using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class ImageParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "img", "image" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var link = node.Attributes["src"]?.Value;
            var alt = node.Attributes["alt"]?.Value;
            if (link is null)
            {
                generated = Array.Empty<IMdElement>();
                return false;
            }
            var title = node.Attributes["title"]?.Value;

            generated = new[] { new Image(alt, link, title) };
            return true;
        }
    }
}
