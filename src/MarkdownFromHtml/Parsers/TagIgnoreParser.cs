using MarkdownFromHtml.MdElements;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class TagIgnoreParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "title", "meta", "link", "script", "style" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();
            return true;
        }
    }
}
