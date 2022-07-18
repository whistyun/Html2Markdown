using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // ~~text~~ => <del>text</del>
    //
    public class Deleted : AccessoryInline
    {
        public Deleted(IEnumerable<IMdInline> content) : base("~~", content)
        {
        }
    }
}