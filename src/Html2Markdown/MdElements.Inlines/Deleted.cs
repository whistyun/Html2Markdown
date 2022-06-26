﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Html2Markdown.MdElements.Inlines
{
    //
    // ~~text~~ => <del>text</del>
    //
    internal class Deleted : AccessoryInline
    {
        public Deleted(IEnumerable<IMdInline> content) : base("~~", content)
        {
        }
    }
}