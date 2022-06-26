﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html2Markdown.MdElements.Inlines
{
    //
    // **text**  => <b>text</b>
    //              <strong>text</strong>
    //
    internal class Bold : AccessoryInline
    {
        public Bold(IEnumerable<IMdInline> content) : base("**", content)
        {
        }
    }
}