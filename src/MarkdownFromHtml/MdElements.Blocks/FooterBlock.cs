using MarkdownFromHtml.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class FooterBlock : IMdBlock
    {
        public IEnumerable<IMdBlock> Content { get; }

        public FooterBlock(IEnumerable<IMdBlock> content)
        {
            Content = content;
        }

        public IEnumerable<string> ToMarkdown()
        {
            bool isRepeated = false;

            foreach (var block in Content)
            {
                if (isRepeated)
                {
                    // insert empty line
                    yield return "^^ ";
                }

                foreach (var line in block.ToMarkdown().SelectMany(ln => ln.SplitLine()))
                {
                    yield return "^^ " + line;
                }
            }
        }
    }
}
