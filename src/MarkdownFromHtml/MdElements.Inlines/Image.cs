using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // ![text](link) => <img alt="text" src="link">
    //
    public class Image : IMdInline
    {
        public string? Alt { get; }
        public string Src { get; }
        public string? Title { get; }

        public Image(string? alt, string src, string? title)
        {
            Alt = alt;
            Src = src;
            Title = title;
        }

        public void TrimStart() => Alt?.TrimStart();

        public void TrimEnd() => Alt?.TrimEnd();

        public bool EndsWithSpace() => false;

        public string ToMarkdown() => $"![{(Alt is null ? "" : Alt)}]({Src}{(Title is null ? "" : @$" ""{Title}""")})";
    }
}