using MarkdownFromHtml.Utils;
using System.Collections.Generic;

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

                foreach (var multiline in block.ToMarkdown())
                    foreach (var line in multiline.SplitLine())
                        yield return "^^ " + line;
            }
        }
    }
}
