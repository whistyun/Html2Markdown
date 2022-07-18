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
        public bool DriveOutSpace { get; }

        public AccessoryInline(string tag, IEnumerable<IMdInline> content) : this(tag, content, true)
        {
        }

        public AccessoryInline(string tag, IEnumerable<IMdInline> content, bool driveOutSpace)
        {
            Tag = tag;
            Content = content;
            DriveOutSpace = driveOutSpace;
        }

        public void TrimStart()
        {
            if (DriveOutSpace) Content.FirstOrDefault()?.TrimStart();
        }

        public void TrimEnd()
        {
            if (DriveOutSpace) Content.LastOrDefault()?.TrimEnd();
        }

        public bool EndsWithSpace()
        {
            if (!DriveOutSpace) return false;

            var end = Content.LastOrDefault();

            if (end is null) return false;

            return end.EndsWithSpace();
        }



        public virtual string ToMarkdown()
        {
            var buff = new StringBuilder();


            if (DriveOutSpace)
            {
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
            }
            else
            {
                buff.Append(Tag);

                foreach (var cnt in Content)
                    buff.Append(cnt.ToMarkdown());

                buff.Append(Tag);
            }

            return buff.ToString();
        }
    }
}
