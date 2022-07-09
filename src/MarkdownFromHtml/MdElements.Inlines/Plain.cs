using System;
using System.Text;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Inlines
{
    public class Plain : IMdInline
    {
        public string Content { private set; get; }

        public Plain(string text)
        {
            bool alreadySpaced = false;

            var buff = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; ++i)
            {
                var c = text[i];

                if (Char.IsWhiteSpace(c))
                {
                    if (alreadySpaced) continue;

                    buff.Append(' ');

                    alreadySpaced = true;
                    continue;
                }

                alreadySpaced = false;

                if (c == '&' && text.TryDecode(ref i, out var decoded))
                {
                    buff.Append(decoded);
                    continue;
                }

                buff.Append(c);
            }

            Content = buff.ToString();
        }

        public void TrimStart() => Content = Content.TrimStart();

        public void TrimEnd() => Content = Content.TrimEnd();

        public string ToMarkdown() => Content;
    }
}
