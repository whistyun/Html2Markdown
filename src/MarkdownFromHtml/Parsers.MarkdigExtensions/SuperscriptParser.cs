using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace MarkdownFromHtml.Parsers.MarkdigExtensions
{
    public class SuperscriptParser : SimpleInlineParser, IRequestEscapeCharacter
    {
        public SuperscriptParser() : base("sup") { }

        public IEnumerable<char> EscapeCharTarget => new[] { '^' };

        public override bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
            => Parse(node, manager, nds => new Superscript(nds), out generated);
    }
}
