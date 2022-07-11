using MarkdownFromHtml.MdElements.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Utils
{
    class TableCellMeasure
    {
        private List<Span> _spans = new();
        private List<Hold> _holds = new();
        private List<GridTableCell> _list = new();
        private Dictionary<GridTableCell, CellPos> _pos = new();
        private int _detailsStartPosition = -1;
        private int _colLenSum = 0;

        public CellPos Current { get; set; } = new(0, 0);
        public LenientList<int> ColLenList { get; } = new(3);
        public LenientList<int> RowLenList { get; } = new(1);


        public void Add(GridTableCell cell)
        {
            _list.Add(cell);

            if (cell.ColSpan > 1 || cell.RowSpan > 1)
            {
                _holds.Add(new Hold(Current, cell));
            }
            else
            {
                RowLenList.SetIfMax(Current.RowAt, cell.RowLen);
                ColLenList.SetIfMax(Current.ColAt, cell.ColLen);
            }

            _pos[cell] = Current;
            MoveRight(cell.ColSpan);

            if (cell.RowSpan > 1)
            {
                _spans.Add(new Span(_pos[cell], cell));
            }
        }

        public void NewRow()
        {
            foreach (var s in _spans.ToArray())
            {
                s.Remains--;

                if (s.Remains == 0)
                    _spans.Remove(s);
            }

            Current = new CellPos(Current.RowAt + 1, 0);
            MoveRight();
        }

        public void EndHead()
        {
            _detailsStartPosition = Current.RowAt;
        }

        private void MoveRight(int right = 0)
        {
            int colAt = Current.ColAt + right;

            foreach (var s in _spans)
            {
                if (s.ColAt <= colAt && colAt < s.ColAt + s.ColSpan)
                    colAt = s.ColAt + s.ColSpan;
            }

            Current = new CellPos(Current.RowAt, colAt);
        }

        public void Finish(int[]? colwidratio)
        {
            // expand width with ratio
            LenientList<int>? colWidRatio = null;

            if (colwidratio is not null)
            {
                colWidRatio = new LenientList<int>(colwidratio, colwidratio[colwidratio.Length - 1]);
                colWidRatio.Expand(Math.Max(colWidRatio.Count, ColLenList.Count));

                var sumRatio = colWidRatio.Sum();
                double[] colWidScale = Enumerable.Range(0, colWidRatio.Count)
                                                 .Select(i => ColLenList[i] * sumRatio / (double)colWidRatio[i])
                                                 .ToArray();

                var colWidMax = colWidScale.Max();
                for (int i = 0; i < colWidScale.Length; ++i)
                    ColLenList[i] = (int)Math.Ceiling(colWidMax * colWidRatio[i] / sumRatio);
            }


            foreach (var hold in _holds)
            {
                if (hold.ColSpan > 1)
                {
                    int requestAdd = hold.ColLen
                                   - Enumerable.Range(hold.ColAt, hold.ColSpan)
                                               .Sum(idx => ColLenList[idx])
                                   - (hold.ColSpan - 1);

                    if (requestAdd <= 0) continue;

                    if (colWidRatio is null)
                    {
                        ColLenList[hold.ColAt + hold.ColSpan - 1] += requestAdd;
                    }
                    else
                    {
                        var scales = Enumerable.Range(hold.ColAt, hold.ColSpan)
                                               .Select(idx => colWidRatio[idx]);
                        int totalScale = scales.Sum();

                        int[] adding = scales.Select(scl => requestAdd * scl / totalScale).ToArray();

                        int remains = requestAdd - adding.Sum();
                        foreach (var e in adding.Select((val, idx) => (val, idx))
                                                .OrderByDescending(e => e.val))
                        {
                            adding[e.idx]++;
                            if (--remains == 0) break;
                        }

                        for (int i = 0; i < adding.Length; ++i)
                            ColLenList[hold.ColAt + i] += adding[i];
                    }
                }
                else ColLenList.SetIfMax(hold.ColAt, hold.ColLen);
            }

            foreach (var hold in _holds)
            {
                if (hold.RowSpan > 1)
                {
                    int requestAdd = hold.RowLen
                                   - Enumerable.Range(hold.RowAt, hold.RowSpan)
                                               .Sum(idx => RowLenList[idx])
                                   - (hold.RowSpan - 1);

                    if (requestAdd <= 0) continue;


                    RowLenList[hold.RowAt + hold.RowSpan - 1] += requestAdd;
                }
                else RowLenList.SetIfMax(hold.RowAt, hold.RowLen);
            }


            // reexpand width with ratio
            if (_holds.Count > 0 && colWidRatio is not null)
            {
                var sumRatio = colWidRatio.Sum();

                double[] colWidScale = Enumerable.Range(0, colWidRatio.Count)
                                                 .Select(i => ColLenList[i] * sumRatio / (double)colWidRatio[i])
                                                 .ToArray();

                var colWidMax = colWidScale.Max();
                for (int i = 0; i < colWidScale.Length; ++i)
                    ColLenList[i] = (int)Math.Ceiling(colWidMax * colWidRatio[i] / sumRatio);
            }

            _colLenSum = ColLenList.Sum() + ColLenList.Count - 1;
        }


        public List<string> CreateText(List<string?> styles)
        {
            var writer = new TableCellWriter(RowLenList, ColLenList, styles, _detailsStartPosition);


            foreach (var cell in _list)
            {
                CellPos pos = _pos[cell];
                writer.Write(pos, cell);
            }

            return writer.ToList();
        }


        class Span
        {
            public CellPos Pos { get; }
            public int Remains { set; get; }
            public int ColSpan { get; }

            public int ColAt => Pos.ColAt;
            public int RowAt => Pos.RowAt;

            public Span(CellPos pos, GridTableCell cell)
            {
                Pos = pos;
                Remains = cell.RowSpan;
                ColSpan = cell.ColSpan;
            }
        }

        class Hold
        {
            private GridTableCell _cell;

            public CellPos Pos { get; }
            public int RowSpan => _cell.RowSpan;
            public int ColSpan => _cell.ColSpan;
            public int RowLen => _cell.RowLen;
            public int ColLen => _cell.ColLen;

            public int ColAt => Pos.ColAt;
            public int RowAt => Pos.RowAt;

            public Hold(CellPos pos, GridTableCell cell)
            {
                Pos = pos;
                _cell = cell;
            }
        }
    }
}
