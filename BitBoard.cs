using System;

namespace ChessMate
{
    [Serializable]
    public static class BitBoard
    {
        // Pieces are stored in a top down bitboard approach
        public static ulong blackPawn = 0x000000000000FF00;
        public static ulong blackRook = 0x0000000000000081;
        public static ulong blackKnight = 0x0000000000000024;
        public static ulong blackBishop = 0x0000000000000042;
        public static ulong blackKing = 0x0000000000000010;
        public static ulong blackQueen = 0x0000000000000008;

        public static ulong whitePawn = 0x00FF000000000000;
        public static ulong whiteRook = 0x1800000000000000;
        public static ulong whiteKnight = 0x2400000000000000;
        public static ulong whiteBishop = 0x4200000000000000;
        public static ulong whiteKing = 0x1000000000000000;
        public static ulong whiteQueen = 0x0800000000000000;

        public static ulong allPieces = 0xFFFF00000000FFFF;
        static ulong allBlackPieces = 0x000000000000FFFF;
        static ulong allWhitePieces = 0xFFFF000000000000;

        static readonly ulong[] bitBoardSquares = new ulong[64];

        // SWAR masks for popcount
        const ulong k1 = 0x5555555555555555; /*  -1/3   */
        const ulong k2 = 0x3333333333333333; /*  -1/5   */
        const ulong k4 = 0x0f0f0f0f0f0f0f0f; /*  -1/17  */
        const ulong kf = 0x0101010101010101; /*  -1/255 */

        public static void Init()
        {
            // Dynamic type for multiple initializations
            for (dynamic i = 0, sqrPos = (ulong)0x1; i < 64; i++, sqrPos <<= 1)
            {
                bitBoardSquares[i] = sqrPos;
            }
        }

        public static int LS1BToSquarePosition(ulong bb)
        {
            // Table of indices corresponding to the LSB in a 64-bit number
            int[] index64 = {0, 47,  1, 56, 48, 27,  2, 60,
                            57, 49, 41, 37, 28, 16,  3, 61,
                            54, 58, 35, 52, 50, 42, 21, 44,
                            38, 32, 29, 23, 17, 11,  4, 62,
                            46, 55, 26, 59, 40, 36, 15, 53,
                            34, 51, 20, 43, 31, 22, 10, 45,
                            25, 39, 14, 33, 19, 30,  9, 24,
                            13, 18,  8, 12,  7,  6,  5, 63};

            /*
             * bitScanForward
             * @param bb bitboard to scan
             * @precondition bb != 0
             * @return index (0..63) of least significant one bit
             */
            // De Bruijn constant for 64-bit
            const ulong debruijn64 = 0x03f79d71b4cb0a89;

            // ((bb ^ (bb-1)) * debruijn64) >> 58 calculates the index of the LSB
            return index64[((bb ^ (bb - 1)) * debruijn64) >> 58];
        }

        public static uint MS1BToSquarePostion(ulong bb)
        {
            // Smear
            bb |= bb >> 1;
            bb |= bb >> 2;
            bb |= bb >> 4;
            bb |= bb >> 8;
            bb |= bb >> 16;
            bb |= bb >> 32;

            // Count the ones
            bb -= bb >> 1 & 0x5555555555555555;
            bb = (bb >> 2 & 0x3333333333333333) + (bb & 0x3333333333333333);
            bb = (bb >> 4) + bb & 0x0f0f0f0f0f0f0f0f;
            bb += bb >> 8;
            bb += bb >> 16;
            bb += bb >> 32;

            return (uint)(bb & 0x0000007f) - 1; // subtract # of 1s from 64
        }

        public static int PopulationCount(ulong x)
        {
            x = x - ((x >> 1) & k1); /* put count of each 2 bits into those 2 bits */
            x = (x & k2) + ((x >> 2) & k2); /* put count of each 4 bits into those 4 bits */
            x = (x + (x >> 4)) & k4; /* put count of each 8 bits into those 8 bits */
            x = (x * kf) >> 56; /* returns 8 most significant bits of x + (x<<8) + (x<<16) + (x<<24) + ...  */
            return (int)x;
        }

        public static ulong SwapBytes(ulong b)
        {
            b = (b & 0x5555555555555555) << 1 | ((b >> 1) & 0x5555555555555555);
            b = (b & 0x3333333333333333) << 2 | ((b >> 2) & 0x3333333333333333);
            b = (b & 0x0f0f0f0f0f0f0f0f) << 4 | ((b >> 4) & 0x0f0f0f0f0f0f0f0f);
            b = (b & 0x00ff00ff00ff00ff) << 8 | ((b >> 8) & 0x00ff00ff00ff00ff);

            return (b << 48) | ((b & 0xffff0000) << 16) | ((b >> 16) & 0xffff0000) | (b >> 48);

        }

        public static ulong VerticalFlip(ulong x)
        {
            const ulong k1 = 0x00FF00FF00FF00FF;
            const ulong k2 = 0x0000FFFF0000FFFF;
            x = ((x >> 8) & k1) | ((x & k1) << 8);
            x = ((x >> 16) & k2) | ((x & k2) << 16);
            x = (x >> 32) | (x << 32);

            return x;
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

        public static void PrintBBRepresentation(ulong positions)
        {
            for(int i = 0; i < 64; ++i)
            {
                Console.Write(((positions >> i) & 1) + " ");
                if((i+1)%8 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        public static void PrintBoard()
        {
            for (int i = 0; i < 64; ++i)
            {
                Console.Write("{0:X16} ", bitBoardSquares[i]);
                if ((i + 1) % 8 == 0)
                {
                    Console.WriteLine("\n");
                }
            }
        }

    }
}
