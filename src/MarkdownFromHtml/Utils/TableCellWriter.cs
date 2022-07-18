using MarkdownFromHtml.MdElements.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Utils
{
    class TableCellWriter
    {
        private int[] _colPosAry { get; }
        private int[] _rowPosAry { get; }
        private int[] _colLenAry { get; }
        private int[] _rowLenAry { get; }
        private List<StringBuilder> _texts;

        public TableCellWriter(
            IEnumerable<int> rowLenList,
            IEnumerable<int> colLenList,
            List<string?> styles,
            int detailsStartPos)
        {
            _rowLenAry = rowLenList.ToArray();
            _colLenAry = colLenList.ToArray();

            _rowPosAry = CreatePosAry(_rowLenAry);
            _colPosAry = CreatePosAry(_colLenAry);

            _texts = CreateBlankTable(
                        _rowLenAry,
                        _colLenAry,
                        styles,
                        detailsStartPos);
        }

        public void Write(CellPos pos, GridTableCell cell)
        {
            // fill blank according to rowpan and colspan

            //// clear column splitter
            foreach (int lineAt in Enumerable.Range(pos.RowAt, cell.RowSpan)
                                             .SelectMany(ridx => Enumerable.Range(_rowPosAry[ridx], _rowLenAry[ridx])))
            {
                StringBuilder buff = _texts[lineAt];

                foreach (int columnAt in Enumerable.Range(pos.ColAt + 1, cell.ColSpan - 1)
                                                   .Select(cidx => _colPosAry[cidx] - 1))
                {
                    buff[columnAt] = ' ';
                }
            }

            //// clear row splitter
            foreach (int lineAt in Enumerable.Range(pos.RowAt + 1, cell.RowSpan - 1)
                                             .Select(ridx => _rowPosAry[ridx] - 1))
            {
                StringBuilder buff = _texts[lineAt];


                int columnLen = Enumerable.Range(pos.ColAt, cell.ColSpan)
                                          .Sum(cidx => _colLenAry[cidx])
                              + cell.ColSpan - 1;

                foreach (int columnAt in Enumerable.Range(_colPosAry[pos.ColAt], columnLen))
                {
                    buff[columnAt] = ' ';
                }
            }


            // write content
            {
                int columnStart = _colPosAry[pos.ColAt];

                int lineAt = _rowPosAry[pos.RowAt];
                foreach (var line in cell.Markdown)
                {
                    StringBuilder buff = _texts[lineAt++];

                    int columnAt = columnStart;
                    foreach (var ch in line)
                    {
                        buff[columnAt++] = ch;
                    }
                }

            }


        }

        public List<string> ToList() => _texts.Select(b => b.ToString()).ToList();


        private static int[] CreatePosAry(int[] lenAry)
        {
            var atAry = new int[lenAry.Length];
            atAry[0] = 1;
            for (int i = 1; i < atAry.Length; ++i)
                atAry[i] = atAry[i - 1] + lenAry[i - 1] + 1;

            return atAry;
        }

        private static List<StringBuilder> CreateBlankTable(
            int[] rowLenAry,
            int[] colLenAry,
            List<string?> styles,
            int detailsStartPos)
        {
            int colLenSum = colLenAry.Sum() + colLenAry.Length - 1;
            var texts = new List<StringBuilder>(rowLenAry.Sum() + rowLenAry.Length - 1);

            texts.Add(CreateStyleRowSeparator(colLenAry, colLenSum, styles));

            for (int i = 0; i < rowLenAry.Length; ++i)
            {
                foreach (var _ in Enumerable.Range(0, rowLenAry[i]))
                    texts.Add(CreateEmptyRow(colLenAry, colLenSum));

                var sep = (i + 1 == detailsStartPos) ? '=' : '-';
                texts.Add(CreateRowSeparator(colLenAry, colLenSum, sep));
            }

            return texts;


            static StringBuilder CreateStyleRowSeparator(int[] colLenAry, int colLenSum, List<string?> styles)
            {
                var buff = new StringBuilder(colLenSum);

                buff.Append("+");
                for (int i = 0; i < colLenAry.Length; ++i)
                {
                    int len = colLenAry[i];

                    switch (i < styles.Count ? styles[i] : null)
                    {
                        default:
                            for (int j = 0; j < len; ++j) buff.Append("-");
                            break;

                        case "left":
                            buff.Append(":");
                            for (int j = 0; j < len - 1; ++j) buff.Append("-");
                            break;

                        case "right":
                            for (int j = 0; j < len - 1; ++j) buff.Append("-");
                            buff.Append(":");
                            break;

                        case "center":
                            buff.Append(":");
                            for (int j = 0; j < len - 2; ++j) buff.Append("-");
                            buff.Append(":");
                            break;
                    };
                    buff.Append("+");
                }
                return buff;
            }

            static StringBuilder CreateRowSeparator(int[] colLenAry, int colLenSum, char sep = '-')
            {
                var buff = new StringBuilder(colLenSum);

                buff.Append("+");
                for (int i = 0; i < colLenAry.Length; ++i)
                {
                    for (int j = 0; j < colLenAry[i]; ++j)
                        buff.Append(sep);

                    buff.Append("+");
                }

                return buff;
            }

            static StringBuilder CreateEmptyRow(int[] colLenAry, int colLenSum)
            {
                var buff = new StringBuilder(colLenSum);

                buff.Append('|');
                for (int i = 0; i < colLenAry.Length; ++i)
                {
                    for (int j = 0; j < colLenAry[i]; ++j)
                        buff.Append(' ');

                    buff.Append("|");
                }

                return buff;
            }
        }
    }
}
