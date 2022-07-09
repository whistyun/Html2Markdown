using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class HeadingBlock : IMdBlock
    {
        public int Level { get; }
        public IMdBlock Content { get; }


        public HeadingBlock(int level, IMdBlock content)
        {
            Level = level;
            Content = content;
        }

        public IEnumerable<string> ToMarkdown()
        {
            var heading = new String('#', Level);

            foreach (var line in Content.ToMarkdown())
                yield return heading + " " + line;
        }
    }
}
