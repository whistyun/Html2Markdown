using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.MdElements.Inlines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarkdownFromHtml.Parsers
{
    public class CodeSpanParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "code" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            // inline code support only Plain
            if (node.ChildNodes.All(e => e is HtmlCommentNode or HtmlTextNode))
            {
                var span = node.InnerText;

                int tagCnt = 1;
                var mch = Regex.Match(span, "`+");
                if (mch.Success)
                {
                    tagCnt = 1 + mch.Value.Length;
                }

                generated = new[] { new Code(new String('`', tagCnt), span) };
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
