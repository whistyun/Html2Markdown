using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml
{
    public class UnknownTagException : Exception
    {
        public string TagName { get; }
        public string Content { get; }

        public UnknownTagException(HtmlNode node) : base($"unknown tag: {node.Name}")
        {
            TagName = node.Name;
            Content = node.OuterHtml;
        }

    }
}
