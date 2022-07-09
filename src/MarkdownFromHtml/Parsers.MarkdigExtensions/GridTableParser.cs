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

namespace MarkdownFromHtml.Parsers.MarkdigExtensions
{
    public class GridTableParser : ISimpleTagParser
    {
        public IEnumerable<string> SupportTag => new[] { "table" };

        public bool TryReplace(HtmlNode node, ReplaceManager manager, out IEnumerable<IMdElement> generated)
        {
            generated = Array.Empty<IMdElement>();

            var cols = ParseWidth(node.ChildNodes.CollectTag("col"));
            if (cols is null) return false;

            // grid table can has no body or no head.

            var theadRows = node.SelectNodes("./thead/tr");
            var tbodyRows = node.SelectNodes("./tbody/tr");
            var firstRow = theadRows ?? tbodyRows;
            if (firstRow is null) return false;

            List<GridTableCell>[]? headGrp = null;
            List<GridTableCell>[]? bodyGrp = null;

            if (theadRows is not null)
            {
                headGrp = TableRows2Block(theadRows, manager);
                if (headGrp is null)
                    return false;
            }

            if (tbodyRows is not null)
            {
                bodyGrp = TableRows2Block(tbodyRows, manager);
                if (bodyGrp is null)
                    return false;
            }

            var headStyle = ParseColumnStyle(firstRow.First());

            generated = new[] { new GridTableBlock(cols, headStyle, headGrp, bodyGrp) };
            return true;
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

        private int[]? ParseWidth(List<HtmlNode> nodes)
        {
            if (nodes.Count == 0) return null;

            var parsed = new double[nodes.Count];

            int power = 0;
            for (int i = 0; i < parsed.Length; ++i)
            {
                var attr = nodes[i].Attributes["style"];
                if (attr is null) return null;

                var mch = Regex.Match(attr.Value, "width:([^;]+)%");
                if (!mch.Success) return null;

                var numTxt = mch.Groups[1].Value;
                if (double.TryParse(numTxt, out var num) && num > 0d)
                {
                    var dotPos = numTxt.IndexOf('.');
                    if (dotPos != -1)
                        power = Math.Max(power, numTxt.Length - (dotPos + 1));

                    parsed[i] = num;
                    continue;
                }
                return null;
            }
            var decade = Math.Pow(10, power);

            var round = new int[parsed.Length];
            for (int i = 0; i < parsed.Length; ++i)
            {
                round[i] = (int)(parsed[i] * decade);
            }

            var devide = BcdArray(round);
            for (int i = 0; i < parsed.Length; ++i)
            {
                round[i] /= devide;
            }


            return round;


            static int BcdArray(int[] nums)
            {
                if (nums.Length == 1) return nums[0];
                if (nums.Length == 2) return Bcd(nums[0], nums[1]);

                var bcd = Bcd(nums[0], nums[1]);
                for (int i = 2; i < nums.Length; ++i)
                {
                    bcd = Bcd(bcd, nums[i]);
                }

                return bcd;
            }

            static int Bcd(int a, int b)
            {
                while (a != 0 && b != 0)
                {
                    if (a > b)
                        a %= b;
                    else
                        b %= a;
                }

                return a | b;
            }
        }

        private List<GridTableCell>[]? TableRows2Block(IEnumerable<HtmlNode> rows, ReplaceManager manager)
        {
            static int ParseOr1(string? txt) => Int32.TryParse(txt, out var num) ? num : 1;

            var list = new List<List<GridTableCell>>();

            foreach (var row in rows)
            {
                List<GridTableCell> cells = new();

                foreach (var cell in row.ChildNodes.CollectTag())
                {
                    int colspan = ParseOr1(cell.Attributes["colspan"]?.Value);
                    int rowspan = ParseOr1(cell.Attributes["rowspan"]?.Value);

                    var parsed = manager.ParseAndGroup(cell.ChildNodes);

                    cells.Add(new GridTableCell(rowspan, colspan, parsed));
                }

                list.Add(cells);
            }

            return list.ToArray();
        }
    }
}
