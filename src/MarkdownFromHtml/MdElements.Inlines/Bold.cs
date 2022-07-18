using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // **text**  => <b>text</b>
    //              <strong>text</strong>
    //
    public class Bold : AccessoryInline
    {
        public Bold(IEnumerable<IMdInline> content) : base("**", content)
        {
        }
    }
}