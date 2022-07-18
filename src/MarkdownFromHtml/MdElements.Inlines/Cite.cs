using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // ""text""  => <cite>text</cite>
    //
    public class Cite : AccessoryInline
    {
        public Cite(IEnumerable<IMdInline> content) : base("\"\"", content)
        {
        }
    }
}