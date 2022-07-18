using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // ==text== => <mark>text</mark>
    //
    public class Marked : AccessoryInline
    {
        public Marked(IEnumerable<IMdInline> content) : base("==", content)
        {
        }
    }
}