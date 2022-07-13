using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class UnorderListParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "ul" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var block = new UnorderListBlock();

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
