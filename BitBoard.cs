using System;

namespace ChessMate
{
    [Serializable]
    public static class BitBoard
    {
        // Pieces are stored in a top down bitboard approach
        public static ulong blackPawn = 0x000000000000FF00;
        public static ulong blackRook = 0x0000000000000081;
        public static ulong blackKnight = 0x0000000000000042;
        public static ulong blackBishop = 0x0000000000000024;
        public static ulong blackKing = 0x0000000000000010;
        public static ulong blackQueen = 0x0000000000000008;

        public static ulong whitePawn = 0x00FF000000000000;
        public static ulong whiteRook = 0x8100000000000000;
        public static ulong whiteKnight = 0x4200000000000000;
        public static ulong whiteBishop = 0x0000000000000024;
        public static ulong whiteKing = 0x0000000000000010;
        public static ulong whiteQueen = 0x0000000000000008;

        static ulong allPieces = 0xFFFF00000000FFFF;
        static ulong allBlackPieces = 0x000000000000FFFF;
        static ulong allWhitePieces = 0xFFFF000000000000;

        static readonly ulong[] bitBoardSquares = new ulong[64];

        public static void Init()
        {
            // Dynamic type for multiple initializations
            for (dynamic i = 0, sqrPos = (ulong)0x1; i < 64; i++, sqrPos <<= 1)
            {
                bitBoardSquares[i] = sqrPos;
            }
        }

        public static ulong UpdateAllPieces(ulong oldPos, ulong newPiece)
        {
            allPieces = allPieces ^ oldPos | newPiece;
            return allPieces;
        }
        
        public static ulong GetUnoccipied()
        {
            return ~allPieces;
        }

        public static bool IsOccupied(int square)
        {
            return (bitBoardSquares[square] & allPieces) != 0;
        }

        public static void PrintBoard()
        {
            for (int i = 0; i < 64; ++i)
            {
                Console.Write("{0:X16} ", bitBoardSquares[i]);
                if((i+1) % 8 == 0)
                {
                    Console.WriteLine("\n");
                }
            }
        }

    }
}
