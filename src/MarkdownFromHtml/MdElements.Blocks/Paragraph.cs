using MarkdownFromHtml.MdElements.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class Paragraph : IMdBlock
    {
        public IEnumerable<IMdInline> Inlines { get; }

        public Paragraph(IEnumerable<IMdInline> inlines)
        {
            Inlines = inlines;
        }

        public IEnumerable<string> ToMarkdown()
        {
            var buff = new StringBuilder();
            foreach (var inline in Inlines)
                buff.Append(inline.ToMarkdown());

            yield return buff.ToString();
        }
    }
}
