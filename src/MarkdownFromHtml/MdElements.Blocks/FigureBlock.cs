using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class FigureBlock : IMdBlock
    {
        public Paragraph Caption { get; }
        public IEnumerable<IMdBlock> Content { get; }

        public FigureBlock(Paragraph caption, IEnumerable<IMdBlock> content)
        {
            Caption = caption;
            Content = content;
        }

        public IEnumerable<string> ToMarkdown()
        {
            yield return "^^^";
            foreach (var block in Content)
                foreach (var line in block.ToMarkdown())
                    yield return line;

            yield return "^^^ " + (Caption.ToMarkdown().FirstOrDefault() ?? "");
        }
    }
}
