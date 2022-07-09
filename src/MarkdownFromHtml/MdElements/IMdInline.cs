using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownFromHtml.MdElements
{
    public interface IMdInline : IMdElement
    {
        void TrimStart();
        void TrimEnd();

        string ToMarkdown();
    }
}
