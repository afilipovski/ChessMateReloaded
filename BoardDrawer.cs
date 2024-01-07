using System;
using System.Drawing;

namespace ChessMate
{
    public static class BoardDrawer
    {
        public static int SQUARE_SIZE { get; set; }
        public static int OFFSET_X { get; set; }
        public static int OFFSET_Y { get; set; } = 25;
        public static void DrawBoardTiles(Graphics g, int height, int width)
        {
            SQUARE_SIZE = (height - OFFSET_Y) / 8;
            OFFSET_X = (width - 8 * SQUARE_SIZE) / 2;

            for (int X = 0; X < 8; ++X)
            {
                for (int Y = 0; Y < 8; ++Y)
                {
                    Brush b = new SolidBrush(((X + Y) % 2 == 0) ? Color.White : Color.DarkSlateGray);
                    g.FillRectangle(b, X * SQUARE_SIZE + OFFSET_X, Y * SQUARE_SIZE + OFFSET_Y, SQUARE_SIZE, SQUARE_SIZE);
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), X * SQUARE_SIZE + OFFSET_X, Y * SQUARE_SIZE + OFFSET_Y, SQUARE_SIZE, SQUARE_SIZE);
                    b.Dispose();
                }
            }
        }

        public static void DrawBitBoard(Graphics g, BitBoard b)
        {
            int numPieces = 12;
            for(int i = 0; i < numPieces; i++) {
                b.pieces[i].Draw(g);
            }
        }

        public static void DrawPossibleMoves(Graphics g, ulong moves)
        {
            ulong positions = moves;
            while (positions > 0)
            {
                int pos = BitBoardUtils.LS1BToSquarePosition(positions);
                int X = pos % 8;
                int Y = pos / 8;

                g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 255, 0)),
                    X * SQUARE_SIZE + OFFSET_X, Y * SQUARE_SIZE + OFFSET_Y,
                    SQUARE_SIZE, SQUARE_SIZE);

                positions &= positions - 1;
            }
            
        }

        public static void PrintBBRepresentation(ulong positions)
        {
            for (int i = 0; i < 64; ++i)
            {
                Console.Write(((positions >> i) & 1) + " ");
                if ((i + 1) % 8 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
    }
}
