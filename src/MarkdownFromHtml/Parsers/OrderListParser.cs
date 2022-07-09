using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class OrderListParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "ol" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var block = new OrderListBlock();

            var startAttr = node.Attributes["start"];
            if (startAttr is not null && Int32.TryParse(startAttr.Value, out var start))
            {
                block.Start = start;
            }

            foreach (var listItem in node.ChildNodes.CollectTag("li"))
            {
                var items = manager.ParseAndGroup(listItem.ChildNodes);
                block.ListItems.Add(items);
            }

            generated = block.ListItems.Count > 0 ?
                            new[] { block } :
                            Array.Empty<IMdElement>();

            return true;
        }
    }
}
