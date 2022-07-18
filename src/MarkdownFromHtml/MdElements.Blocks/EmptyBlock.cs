using System;
using System.Collections.Generic;

namespace MarkdownFromHtml.MdElements.Blocks
{
    internal class EmptyBlock : IMdBlock
    {
        public static readonly EmptyBlock Instance = new();

        private EmptyBlock() { }

        public IEnumerable<string> ToMarkdown()
        {
            return Array.Empty<string>();
        }
    }
}
