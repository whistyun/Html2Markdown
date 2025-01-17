﻿using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers.MarkdigExtensions
{
    public class SubscriptParser : SimpleInlineParser, IRequestEscapeCharacter
    {
        public SubscriptParser() : base("sub") { }

        public IEnumerable<char> EscapeCharTarget => new[] { '~' };

        public override bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
            => Parse(node, manager, nds => new Subscript(nds), out generated);
    }
}
