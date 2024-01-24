using System.Collections.Generic;
using System.Drawing;

namespace ChessMate.Pieces
{
    public class KnightBB : PieceBB
    {
        public KnightBB(ulong position, bool white) : base(position, white)
        {
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return white ? Properties.Resources.w_knight_png_shadow_1024px
                : @Properties.Resources.b_knight_png_shadow_1024px;
        }

        public override List<BitBoard> PossibleMoves(BitBoard bb)
        {
            int square = BitBoardUtils.LS1BToSquarePosition(position);
            ulong moves = PieceBBUtils.kingMoves[square];
            return moves;
        }
    }
}
