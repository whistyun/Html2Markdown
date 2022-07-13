using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    public abstract class AccessoryInline : IMdInline
    {
        public string Tag { get; }
        public IEnumerable<IMdInline> Content { get; }

        public AccessoryInline(string tag, IEnumerable<IMdInline> content)
        {
            Tag = tag;
            Content = content;
        }

        public void TrimStart() => Content.FirstOrDefault()?.TrimStart();

        public void TrimEnd() => Content.LastOrDefault()?.TrimStart();

        public virtual string ToMarkdown()
        {
            var buff = new StringBuilder();
            foreach (var cnt in Content)
                buff.Append(cnt.ToMarkdown());


            var headSpaceCounter = 0;
            foreach (var i in Enumerable.Range(0, buff.Length))
            {
                if (Char.IsWhiteSpace(buff[i]))
                {
                    headSpaceCounter++;
                }
                else break;
            }

            var tailSpaceCounter = 0;
            foreach (var i in Enumerable.Range(0, buff.Length).Reverse())
            {
                if (Char.IsWhiteSpace(buff[i]))
                {
                    tailSpaceCounter++;
                }
                else break;
            }

            buff.Insert(headSpaceCounter, Tag);
            if (headSpaceCounter > 1)
            {
                buff.Remove(0, headSpaceCounter - 1);
            }


            buff.Insert(buff.Length - tailSpaceCounter, Tag);
            if (tailSpaceCounter > 1)
            {
                buff.Remove(buff.Length - tailSpaceCounter, tailSpaceCounter - 1);
            }

            return buff.ToString();
        }
    }
}
