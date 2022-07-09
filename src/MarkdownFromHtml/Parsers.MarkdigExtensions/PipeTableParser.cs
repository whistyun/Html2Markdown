using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Parsers
{
    public class PipeTableParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "table" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();

            var theadRows = node.SelectNodes("./thead/tr");
            if (theadRows is null)
                return false;

            var headGrp = TableRows2Block(theadRows, manager);
            if (headGrp is null)
                return false;

            List<IMdBlock>[]? bodyGrp;

            var tbodyRows = node.SelectNodes("./tbody/tr");
            if (tbodyRows is null)
            {
                bodyGrp = Array.Empty<List<IMdBlock>>();
            }
            else
            {
                bodyGrp = TableRows2Block(tbodyRows, manager);
                if (bodyGrp is null)
                    return false;
            }


            List<IMdBlock>[]? footGrp = null;
            var tfootRows = node.SelectNodes("./tfoot/tr");
            if (tfootRows is not null)
            {
                footGrp = TableRows2Block(tfootRows, manager);
                if (footGrp is null)
                    return false;
            }

            var headStyle = ParseColumnStyle(theadRows.First());
            var details = headGrp.Skip(1).Concat(bodyGrp);
            if (footGrp is not null)
            {
                details = details.Concat(footGrp);
            }

            generated = new[] { new PipeTableBlock(headStyle, headGrp.First(), details) };
            return true;
        }

        private List<IMdBlock>[]? TableRows2Block(IEnumerable<HtmlNode> rows, ReplaceManager manager)
        {
            var list = new List<List<IMdBlock>>();

            foreach (var row in rows)
            {
                List<IMdBlock> cells = new();

                foreach (var cell in row.ChildNodes.CollectTag())
                {
                    if (!IsNullOr1(cell.Attributes["colspan"]?.Value)) return null;
                    if (!IsNullOr1(cell.Attributes["rowspan"]?.Value)) return null;

                    var parsed = manager.ParseAndGroup(cell.ChildNodes);
                    if (parsed.Count() > 1) return null;


                    if (parsed.Count() == 0)
                    {
                        // empty cell
                        cells.Add(EmptyBlock.Instance);
                    }
                    else
                    {
                        if (parsed.First() is not Paragraph) return null;
                        cells.Add(parsed.First());
                    }
                }

                list.Add(cells);
            }

            return list.ToArray();
        }

        private List<string?> ParseColumnStyle(HtmlNode row)
        {
            var styles = new List<string?>();

            foreach (var cell in row.ChildNodes.CollectTag())
            {
                var style = cell.Attributes["style"]?.Value;
                if (style is null)
                {
                    styles.Add(null);
                    continue;
                }

                var match = Regex.Match(style, @"text-align[ \t]*:[ \t]*([a-z]+)[ \t]*;?");
                styles.Add(match.Success ? match.Groups[1].Value : null);
            }

            return styles;
        }

        private bool IsNullOr1(string? text)
        {
            if (String.IsNullOrEmpty(text))
                return true;

            if (Int32.TryParse(text, out var num))
                return num == 1;

            return false;
        }
    }
}
