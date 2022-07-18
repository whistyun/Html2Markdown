using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // [text](link) => <a href="link">text</a>
    //
    public class Hyperlink : IMdInline
    {
        public string Link { get; }
        public IEnumerable<IMdInline> Content { get; }
        public string? Title { get; }

        public Hyperlink(IEnumerable<IMdInline> content, string link, string? title)
        {
            Link = link;
            Content = content;
            Title = title;
        }

        public void TrimStart() => Content.FirstOrDefault()?.TrimStart();

        public void TrimEnd() => Content.LastOrDefault()?.TrimStart();

        public bool EndsWithSpace() => false;

        public string ToMarkdown()
        {
            var buff = new StringBuilder();

            buff.Append("[");

            foreach (var cnt in Content)
                buff.Append(cnt.ToMarkdown());

            buff.Append("](");
            buff.Append(Link);
            if (Title is not null)
            {
                buff.Append(@$" ""{Title}""");
            }
            buff.Append(")");

            return buff.ToString();
        }
    }
}