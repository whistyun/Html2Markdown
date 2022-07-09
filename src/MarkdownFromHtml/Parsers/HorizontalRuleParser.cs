using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class HorizontalRuleParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "hr" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = new[] { new HorizontalRuleBlock() };
            return true;
        }
    }
}
