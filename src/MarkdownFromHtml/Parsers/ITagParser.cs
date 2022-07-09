using MarkdownFromHtml.MdElements;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.Parsers
{
    public interface ITagParser
    {
        bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated);
    }

    public interface ISimpleTagParser : ITagParser
    {
        IEnumerable<string> SupportTag { get; }
    }

    public interface IHasPriority
    {
        int Priority { get; }
    }

    public static class TagParserExt
    {
        public const int DefaultPriority = 10000;

        public static int GetPriority(this ITagParser parser)
        {
            return parser is IHasPriority prop ? prop.Priority : DefaultPriority;
        }
    }
}
