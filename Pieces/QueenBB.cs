﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Pieces
{
    internal class QueenBB : PieceBB
    {
        public QueenBB(ulong position, bool white) : base(position, white)
        {
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return white ? Properties.Resources.w_queen_png_shadow_1024px
                : @Properties.Resources.b_queen_png_shadow_1024px;
        }

        public override List<BitBoard> PossibleMoves(BitBoard b)
        {
            throw new NotImplementedException();
        }
    }
}
