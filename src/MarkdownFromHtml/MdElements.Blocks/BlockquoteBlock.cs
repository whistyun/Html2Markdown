using System.Collections.Generic;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class BlockquoteBlock : IMdBlock
    {
        public IEnumerable<IMdBlock> Content { get; }

        public BlockquoteBlock(IEnumerable<IMdBlock> content)
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
                    yield return "> ";
                }

                foreach (var multiline in block.ToMarkdown())
                    foreach (var line in multiline.SplitLine())
                        yield return "> " + line;
            }
        }
    }
}
