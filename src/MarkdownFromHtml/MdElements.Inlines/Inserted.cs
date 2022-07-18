using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // ++text++ => <ins>text</ins>
    //
    public class Inserted : AccessoryInline
    {
        public Inserted(IEnumerable<IMdInline> content) : base("++", content)
        {
        }
    }
}