using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class BoldParser : SimpleInlineParser, IRequestEscapeCharacter, IRequestEscapeString
    {
        public IEnumerable<char> EscapeCharTarget => new[] { '>' };

        public IEnumerable<string> EscapeStringTarget => new[] { "**" };

        public BoldParser() : base("b", "strong") { }

        public override bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
            => Parse(node, manager, nds => new Bold(nds), out generated);
    }
}
