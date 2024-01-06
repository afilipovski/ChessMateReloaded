﻿using System.Collections.Generic;
using System.Drawing;

namespace ChessMate.Pieces
{
    internal class KingBB : PieceBB
    {
        public KingBB(ulong position, bool white) : base(position, white)
        {
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return white ? Properties.Resources.w_king_png_shadow_1024px
                : @Properties.Resources.b_king_png_shadow_1024px;
        }

        public override List<BitBoard> PossibleMoves(Board b)
        {
            throw new System.NotImplementedException();
        }
    }
}
