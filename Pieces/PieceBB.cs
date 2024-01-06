using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Pieces
{
    public abstract class PieceBB
    {
        public ulong position;
        public bool white;

        public PieceBB(ulong position, bool white)
        {
            this.position = position;
            this.white = white;
        }

        public abstract List<BitBoard> PossibleMoves(Board b);

        public abstract Bitmap GetBitmap(Graphics g);

        public void Draw(Graphics g)
        {
            Bitmap bitmap = GetBitmap(g);
            ulong pieces = position;
            while (pieces != 0)
            {
                int pos = BitBoardUtils.LS1BToSquarePosition(pieces);
                int X = pos % 8;
                int Y = pos / 8;
                g.DrawImage(bitmap, X * BoardDrawer.SQUARE_SIZE + BoardDrawer.OFFSET_X, Y * BoardDrawer.SQUARE_SIZE + BoardDrawer.OFFSET_Y, BoardDrawer.SQUARE_SIZE, BoardDrawer.SQUARE_SIZE);
                pieces &= pieces - 1;
            }
        }
    }
}
