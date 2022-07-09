using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // 2^10^ => 2<sup>10</sup>
    //
    public class Superscript : AccessoryInline
    {
        public Superscript(IEnumerable<IMdInline> content) : base("^", content)
        {
        }
    }
}