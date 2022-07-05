﻿using System;

namespace Html2Markdown.Utils
{
    struct CellPos
    {
        public int RowAt { get; }
        public int ColAt { get; }

        public CellPos(int rowAt, int colAt)
        {
            RowAt = rowAt;
            ColAt = colAt;
        }
    }
}
