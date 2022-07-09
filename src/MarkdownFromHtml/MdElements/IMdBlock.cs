using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements
{
    public interface IMdBlock : IMdElement
    {
        IEnumerable<string> ToMarkdown();
    }
}
