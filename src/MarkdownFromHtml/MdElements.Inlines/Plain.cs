using System;
using System.Text;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Inlines
{
    public class Plain : IMdInline
    {
        public string Content { private set; get; }

        public Plain(string text, EscapeManager? man)
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

                if (man is not null && man.IsTarget(text, ref i, out string escaped))
                    buff.Append(escaped);

                else
                    buff.Append(c);
            }

            Content = buff.ToString();
        }

        public void TrimStart() => Content = Content.TrimStart();

        public void TrimEnd() => Content = Content.TrimEnd();

        public bool EndsWithSpace() => Content.Length > 0 && Char.IsWhiteSpace(Content[Content.Length - 1]);

        public string ToMarkdown() => Content;
    }
}
