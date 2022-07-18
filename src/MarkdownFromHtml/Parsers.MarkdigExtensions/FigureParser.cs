using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.MdElements.Inlines;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownFromHtml.Parsers.MarkdigExtensions
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

            var captionInline = manager.ParseJagging(captionList.SelectMany(c => c.ChildNodes));

            if (captionInline.Count() == 1 && captionInline.First() is Paragraph captionPara)
                captionInline = captionPara.Inlines;

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
