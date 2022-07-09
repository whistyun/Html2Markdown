using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public abstract class SimpleInlineParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag { get; }

        public SimpleInlineParser(params string[] tags)
        {
            SupportTag = tags;
        }

        public abstract bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated);

        protected bool Parse(HtmlNode node, ReplaceManager manager, Func<IEnumerable<IMdInline>, IMdInline> converter, out IEnumerable<IMdElement> generated)
        {
            var children = manager.ParseJagging(node.ChildNodes);

            if (children.TryCast<IMdInline>(out var inlines))
            {
                generated = new[] { converter(inlines) };
                return true;
            }

            generated = Array.Empty<IMdElement>();
            return false;
        }
    }
}
