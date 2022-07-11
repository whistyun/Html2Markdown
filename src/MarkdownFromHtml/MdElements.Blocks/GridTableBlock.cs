using MarkdownFromHtml.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class GridTableBlock : IMdBlock
    {
        public List<string?> Styles { get; }
        public IEnumerable<IEnumerable<GridTableCell>>? Header { get; }
        public IEnumerable<IEnumerable<GridTableCell>>? Rows { get; }

        private List<string> _markdown;

        public GridTableBlock(
            int[]? width,
            List<string?> styles,
            IEnumerable<IEnumerable<GridTableCell>>? header,
            IEnumerable<IEnumerable<GridTableCell>>? rows)
        {
            if (header is null && rows is null)
                throw new ArgumentNullException();

            Styles = styles;
            Header = header;
            Rows = rows;

            var demand = new TableCellMeasure();
            if (Header is not null)
            {
                foreach (var row in Header)
                {
                    foreach (var cell in row)
                        demand.Add(cell);

                    demand.NewRow();
                }

                demand.EndHead();
            }

            if (Rows is not null)
                foreach (var row in Rows)
                {
                    foreach (var cell in row)
                        demand.Add(cell);

                    demand.NewRow();
                }

            demand.Finish(width);

            _markdown = demand.CreateText(styles);
        }


        public IEnumerable<string> ToMarkdown() => _markdown;
    }

    public class GridTableCell
    {
        public int RowSpan { get; }
        public int ColSpan { get; }
        public IEnumerable<IMdBlock> Content { get; }

        private Lazy<RectText> _text;
        public string[] Markdown => _text.Value.Text;
        public int ColLen => _text.Value.ColLen;
        public int RowLen => _text.Value.RowLen;


        public GridTableCell(int rowspan, int colspan, IEnumerable<IMdBlock> content)
        {
            RowSpan = rowspan;
            ColSpan = colspan;
            Content = content;

            _text = new Lazy<RectText>(CreateText);
        }

        private RectText CreateText()
        {
            int maxCol = 0;
            var lines = new List<string>();

            using (var blocks = Content.GetEnumerator())
            {
                if (blocks.MoveNext())
                {
                    foreach (var mdline in blocks.Current.ToMarkdown())
                    {
                        maxCol = Math.Max(maxCol, mdline.Length);
                        lines.Add(mdline);
                    }
                }

                while (blocks.MoveNext())
                {
                    lines.Add("");
                    foreach (var mdline in blocks.Current.ToMarkdown())
                    {
                        maxCol = Math.Max(maxCol, mdline.Length);
                        lines.Add(mdline);
                    }
                }
            }

            return new RectText(lines.Count, maxCol, lines.ToArray());
        }

        class RectText
        {
            public int ColLen { get; }
            public int RowLen { get; }
            public string[] Text { get; }

            public RectText(int rowLen, int colLen, string[] text)
            {
                RowLen = rowLen;
                ColLen = colLen;
                Text = text;
            }
        }
    }



}
