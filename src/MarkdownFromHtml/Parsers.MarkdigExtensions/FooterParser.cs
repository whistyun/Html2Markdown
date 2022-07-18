using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers.MarkdigExtensions
{
    public class FooterParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "footer" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var blocks = manager.ParseAndGroup(node.ChildNodes);

            generated = new[] { new FooterBlock(blocks) };
            return true;
        }
    }
}
