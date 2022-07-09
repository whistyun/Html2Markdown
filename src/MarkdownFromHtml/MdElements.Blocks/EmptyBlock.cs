using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    internal class EmptyBlock : IMdBlock
    {
        public static readonly EmptyBlock Instance = new();

        public IEnumerable<string> ToMarkdown()
        {
            yield break;
        }
    }
}
