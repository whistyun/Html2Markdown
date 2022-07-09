using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class PipeTableBlock : IMdBlock
    {
        public List<string?> Styles { get; }
        public IEnumerable<IMdBlock> Header { get; }
        public IEnumerable<IEnumerable<IMdBlock>> Rows { get; }

        public PipeTableBlock(List<string?> styles, IEnumerable<IMdBlock> header, IEnumerable<IEnumerable<IMdBlock>> rows)
        {
            Styles = styles;
            Header = header;
            Rows = rows;
        }

        public IEnumerable<string> ToMarkdown()
        {
            // head
            var headBuff = new StringBuilder();
            headBuff.Append("| ");
            headBuff.Append(String.Join(" | ", Header.Select(head => head.ToMarkdown().First())));
            headBuff.Append(" |");

            yield return headBuff.ToString();


            // style
            string styleTxt(string? style)
                => style?.ToLower() switch { "left" => ":--", "right" => "--:", "center" => ":-:", _ => "---" };

            var styleBuff = new StringBuilder();
            styleBuff.Append("| ");
            styleBuff.Append(String.Join(" | ", Styles.Select(style => styleTxt(style))));
            styleBuff.Append(" |");

            yield return styleBuff.ToString();


            // details
            var rowBuff = new StringBuilder();
            foreach (var row in Rows)
            {
                rowBuff.Length = 0;
                rowBuff.Append("| ");
                rowBuff.Append(String.Join(" | ", row.Select(head => head.ToMarkdown().FirstOrDefault() ?? "")));
                rowBuff.Append(" |");

                yield return rowBuff.ToString();
            }
        }
    }
}
