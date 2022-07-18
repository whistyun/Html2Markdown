using System.Collections.Generic;
using System.Net;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class IndendBlock : IMdBlock
    {
        public string Code { get; }

        public IndendBlock(string code)
        {
            // trim head & tail linebreak
            if (code.Length > 0 && code[0] == '\n')
                code = code.Substring(1);

            if (code.Length > 0 && code[code.Length - 1] == '\n')
                code = code.Substring(0, code.Length - 1);

            Code = WebUtility.HtmlDecode(code);
        }

        public IEnumerable<string> ToMarkdown()
        {
            foreach (var codeline in Code.SplitLine())
                yield return "    " + codeline;
        }
    }
}
