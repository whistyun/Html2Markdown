using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class Paragraph : IMdBlock
    {
        public IMdInline[] Inlines { get; }

        public Paragraph(IMdInline[] inlines)
        {
            Inlines = inlines;
        }

        public IEnumerable<string> ToMarkdown()
        {
            switch (Inlines.Length)
            {
                case 0:
                    return Array.Empty<string>();

                case 1:
                    return new[] { Inlines[0].ToMarkdown() };

                case 2:
                    return new[] { Inlines[0].ToMarkdown() + Inlines[1].ToMarkdown() };

                case 3:
                    return new[] { Inlines[0].ToMarkdown() + Inlines[1].ToMarkdown() + Inlines[2].ToMarkdown() };

                case 4:
                    return new[] { Inlines[0].ToMarkdown() + Inlines[1].ToMarkdown() + Inlines[2].ToMarkdown() + Inlines[3].ToMarkdown() };

                default:
                    var buff = new StringBuilder();
                    foreach (var inline in Inlines)
                        buff.Append(inline.ToMarkdown());

                    return new[] { buff.ToString() };
            }
        }
    }
}
