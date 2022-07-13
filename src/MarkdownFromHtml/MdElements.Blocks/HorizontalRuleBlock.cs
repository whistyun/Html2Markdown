using System.Collections.Generic;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class HorizontalRuleBlock : IMdBlock
    {
        public IEnumerable<string> ToMarkdown()
        {
            yield return "* * *";
        }
    }
}
