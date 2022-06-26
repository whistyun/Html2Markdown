﻿using Html2Markdown.MdElements;
using Html2Markdown.MdElements.Inlines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Html2Markdown.Parsers
{
    internal class HyperlinkParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "a" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            var link = node.Attributes["href"]?.Value;
            var content = manager.ParseJagging(node.ChildNodes);
            var title = node.Attributes["title"]?.Value;
            if (link is null || !content.All(nd => nd is IMdInline))
            {
                generated = Array.Empty<IMdElement>();
                return false;
            }

            generated = new[] { new Hyperlink(content.Cast<IMdInline>(), link, title) };
            return true;
        }
    }
}
