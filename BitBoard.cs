using ChessMate.Pieces;
using System;
using System.Collections.Generic;

namespace ChessMate
{
    [Serializable]
    public class BitBoard
    {
        // Pieces are stored in a top down bitboard approach
        public PieceBB[] pieces;

        public ulong allPieces = 0xFFFF00000000FFFF;
        public ulong allBlackPieces = 0x000000000000FFFF;
        public ulong allWhitePieces = 0xFFFF000000000000;

        public BitBoard() {
            pieces = new PieceBB[]{
                new PawnBB(0x000000000000FF00, false),
                new RookBB(0x0000000000000081, false),
                new KnightBB(0x0000000000000024, false),
                new BishopBB(0x0000000000000042, false),
                new KingBB(0x0000000000000010,false),
                new QueenBB(0x0000000000000008,false),

                new PawnBB(0x00FF000000000000, true),
                new RookBB(0x8100000000000000, true),
                new KnightBB(0x2400000000000000, true),
                new BishopBB(0x4200000000000000, true),
                new KingBB(0x1000000000000000, true),
                new QueenBB(0x0800000000000000, true)
            };
        }

        public ulong UpdateAllPieces(ulong oldPos, ulong newPiece)
        {
            allPieces = allPieces ^ oldPos | newPiece;
            return allPieces;
        }

        public ulong GetUnoccipied()
        {
            return ~allPieces;
        }

    }
}
