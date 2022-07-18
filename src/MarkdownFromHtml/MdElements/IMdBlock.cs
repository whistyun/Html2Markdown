using System.Collections.Generic;

namespace MarkdownFromHtml.MdElements
{
    public interface IMdBlock : IMdElement
    {
        /// <summary>
        /// Returns texts in markdown syntax.
        /// Text elements may contain linebreak.
        /// Elements are treated as line, So a linebreak may be inserted between text elements.
        /// </summary>
        IEnumerable<string> ToMarkdown();
    }
}
