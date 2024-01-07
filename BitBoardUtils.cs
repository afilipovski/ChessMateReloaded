using ChessMate.Pieces;
using System;

namespace ChessMate
{
    public static class BitBoardUtils
    {
        static readonly ulong[] bitBoardSquares = new ulong[64];

        // Table of indices corresponding to the LSB in a 64-bit number
        static readonly int[] index64 = {0, 47,  1, 56, 48, 27,  2, 60,
                            57, 49, 41, 37, 28, 16,  3, 61,
                            54, 58, 35, 52, 50, 42, 21, 44,
                            38, 32, 29, 23, 17, 11,  4, 62,
                            46, 55, 26, 59, 40, 36, 15, 53,
                            34, 51, 20, 43, 31, 22, 10, 45,
                            25, 39, 14, 33, 19, 30,  9, 24,
                            13, 18,  8, 12,  7,  6,  5, 63};
        // De Bruijn constant for 64-bit
        const ulong debruijn64 = 0x03f79d71b4cb0a89;

        public static void Init()
        {
            // Dynamic type for multiple initializations
            for (dynamic i = 0, sqrPos = (ulong)0x1; i < 64; i++, sqrPos <<= 1)
            {
                bitBoardSquares[i] = sqrPos;
            }
        }

        public static PieceBB FindPieceAtPosition(BitBoard board, ulong x)
        {
            if ((board.allPieces & x) == 0) return null;
            for (int i = 0; i < 12; ++i)
            {
                if ((board.pieces[i].position & x) != 0) return board.pieces[i];
            }
            return null;
        }

        /// <summary>
        /// Bit scan forward, transforming least significant 1 bit to index
        /// </summary>
        /// <param name="bb">bitboard for scan</param>
        /// <returns>index (0..63) of least significant one bit</returns>
        public static int LS1BToSquarePosition(ulong bb)
        {
            // ((bb ^ (bb-1)) * debruijn64) >> 58 calculates the index of the LSB
            return index64[((bb ^ (bb - 1)) * debruijn64) >> 58];
        }

        /// <summary>
        /// Bit scan from behind, transforming most significant 1 bit to index
        /// </summary>
        /// <param name="bb"></param>
        /// <returns>index (0..63) of most significant one bit</returns>
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

        // SWAR masks for popcount
        const ulong k1 = 0x5555555555555555; /*  -1/3   */
        const ulong k2 = 0x3333333333333333; /*  -1/5   */
        const ulong k4 = 0x0f0f0f0f0f0f0f0f; /*  -1/17  */
        const ulong kf = 0x0101010101010101; /*  -1/255 */

        /// <summary>
        /// Count number of pieces in a bitboard
        /// </summary>
        /// <param name="x">bitboard for the population count</param>
        /// <returns>number of pieces</returns>
        public static int PopulationCount(ulong x)
        {
            x -= ((x >> 1) & k1); /* put count of each 2 bits into those 2 bits */
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

        public static ulong SouthOne(ulong b) { return b >> 8; }
        public static ulong NorthOne(ulong b) { return b << 8; }

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
