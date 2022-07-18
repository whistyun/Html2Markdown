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
    public class BlockquoteParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "blockquote" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var blocks = manager.ParseAndGroup(node.ChildNodes);

            generated = new[] { new BlockquoteBlock(blocks) };
            return true;
        }
    }
}
