using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers
{
    public class ItalicParser : SimpleInlineParser, IRequestEscapeCharacter
    {
        public ItalicParser() : base("i", "em") { }

        public IEnumerable<char> EscapeCharTarget => new[] { '*' };

        public override bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
            => Parse(node, manager, nds => new Italic(nds), out generated);
    }
}
