using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class HeadingParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "h1", "h2", "h3", "h4", "h5", "h6" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var level = Int32.Parse(node.Name.Substring(1));

            var blocks = manager.ParseAndGroup(node.ChildNodes);

            if (blocks.Count() == 1 && blocks.First() is Paragraph paragraph)
            {
                generated = new[] { new HeadingBlock(level, paragraph) };
                return true;
            }

            generated = Array.Empty<IMdElement>();
            return false;
        }
    }
}
