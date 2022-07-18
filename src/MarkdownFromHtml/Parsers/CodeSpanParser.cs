using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MarkdownFromHtml.Utils;
using System.Text;

namespace MarkdownFromHtml.Parsers
{
    public class CodeSpanParser : ISimpleTagParser, IRequestEscapeCharacter
    {
        public IEnumerable<string> SupportTag => new[] { "code" };

        public IEnumerable<char> EscapeCharTarget => new[] { '`' };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            // inline code support only Plain
            if (node.ChildNodes.All(e => e is HtmlCommentNode or HtmlTextNode))
            {
                var span = node.InnerText;
                bool driveOut = true;

                if (span[0] == '`')
                {
                    span = " " + span;
                    driveOut = false;
                }
                if (span[span.Length - 1] == '`')
                {
                    span = span + " ";
                    driveOut = false;
                }

                int tagCnt = span.CountContinuous('`') + 1;
                generated = new[] { new Code(new String('`', tagCnt), new Plain(span, null), driveOut) };

                return true;
            }

            // block code support only Plain and Linebreak
            if (HasOnlyPlain(node))
            {
                generated = new[] { new IndendBlock(node.InnerText) };
                return true;
            }

            generated = Array.Empty<IMdElement>();
            return false;
        }

        private bool HasOnlyPlain(HtmlNode node)
            => node.ChildNodes.All(e => e.NodeType switch
            {
                HtmlNodeType.Comment => true,
                HtmlNodeType.Text => true,
                HtmlNodeType.Element => String.Equals(e.Name, "br", StringComparison.OrdinalIgnoreCase),
                _ => false
            });
    }
}
