using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.MdElements.Inlines;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class CodeBlockParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "pre" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();

            var codeElements = node.ChildNodes.CollectTag("code");
            if (codeElements.Count != 0)
            {
                var rslt = new List<IMdElement>();

                foreach (var codeElement in codeElements)
                {
                    // "language-**", "lang-**", "**" or "sourceCode **"
                    var classVal = codeElement.Attributes["class"]?.Value;

                    rslt.Add(new CodeBlock(ParseLangCode(classVal), codeElement.InnerText));
                }

                generated = rslt;
                return rslt.Count > 0;
            }
            else if (node.ChildNodes.TryCastTextNode(out var textNodes))
            {
                var buff = new StringBuilder();
                foreach (var textNode in textNodes)
                    buff.Append(textNode.InnerText);

                generated = new[] {
                    new IndendBlock(buff.ToString())
                };

                return true;
            }
            else return false;
        }

        private string ParseLangCode(string? classVal)
        {
            if (classVal is null) return "";

            // "language-**", "lang-**", "**" or "sourceCode **"
            var indics = Enumerable.Range(0, classVal.Length)
                                   .Reverse()
                                   .Where(i => !Char.IsLetterOrDigit(classVal[i]));

            return classVal.Substring(indics.Count() == 0 ? 0 : indics.First() + 1);
        }
    }
}
