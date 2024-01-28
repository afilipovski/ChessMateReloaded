using System.Collections.Generic;
using System.Drawing;

namespace ChessMate.Pieces
{
    public class PawnBB : PieceBB
    {
        public PawnBB(ulong position, bool white) : base(position, white)
        {
        }

        ulong wPawnsAble2Push(ulong wpawns, ulong unoccupied)
        {
            return BitBoardUtils.SouthOne(unoccupied) & wpawns;
        }

        ulong wPawnsAble2DblPush(ulong wpawns, ulong unoccupied)
        {
            const ulong rank4 = 0x00000000FF000000;
            ulong emptyRank3 = BitBoardUtils.SouthOne(unoccupied & rank4) & unoccupied;
            return wPawnsAble2Push(wpawns, emptyRank3);
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return white ? Properties.Resources.w_pawn_png_shadow_1024px
                : @Properties.Resources.b_pawn_png_shadow_1024px;
        }

        public override List<BitBoard> PossibleMoves(BitBoard b)
        {
            throw new System.NotImplementedException();
        }
    }
}
