﻿using Html2Markdown.MdElements;
using Html2Markdown.MdElements.Blocks;
using Html2Markdown.MdElements.Inlines;
using Html2Markdown.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Html2Markdown.Parsers.MarkdigExtensions
{
    public class FigureParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "figure" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();


            (var captionList, var contentList) =
                node.ChildNodes
                    .SkipComment()
                    .Filter(nd => string.Equals(nd.Name, "figcaption", StringComparison.OrdinalIgnoreCase));

            var captionInline = manager.ParseJagging(contentList);
            if (captionInline.Any(nd => nd is IMdBlock or Linebreak))
                return false;

            var captionBlock = manager.Grouping(captionInline).FirstOrDefault();

            if (captionBlock is Paragraph caption)
            {
                var content = manager.Grouping(manager.ParseJagging(contentList));
                generated = new[] { new FigureBlock(caption, content) };
                return true;
            }

            return false;
        }
    }
}
