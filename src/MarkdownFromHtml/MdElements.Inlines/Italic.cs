using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // **text**  => <b>text</b>
    //              <strong>text</strong>
    //
    public class Italic : AccessoryInline
    {
        public Italic(IEnumerable<IMdInline> content) : base("*", content)
        {
        }
    }
}