using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Inlines
{
    //
    // `text`  => <code>text</code>
    //
    public class Code : AccessoryInline
    {
        public Code(string tag, Plain plain, bool driveOutSpace) : base(tag, new[] { plain }, driveOutSpace)
        {
        }
    }
}