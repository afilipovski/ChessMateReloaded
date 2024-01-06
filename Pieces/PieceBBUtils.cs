using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Pieces
{
    public static class PieceBBUtils
    {
        public static ulong[] knightMoveLookUp = new ulong[64];

        public static ulong[] wPawnAttackLookUp = new ulong[64];
        public static ulong[] bPawnAttackLookUp = new ulong[64];

        public static ulong[] kingMoves = new ulong[64];

        public static void Init()
        {
            for (dynamic i = 0, sqrPos = (ulong)0x1; i < 64; ++i, sqrPos <<= 1)
            {
                ulong left = 0x0;
                ulong right = 0x0;
                ulong up = 0x0;
                ulong down = 0x0;

                if (i % 8 > 0)
                {
                    if (i % 8 > 1)
                    {
                        left = sqrPos >> 2;
                    }
                    up = sqrPos >> 17;
                    down = sqrPos << 15;
                }

                if (i % 8 < 7)
                {
                    if (i % 8 < 6)
                    {
                        right = sqrPos << 2;
                    }
                    up |= (sqrPos >> 15);
                    down |= (sqrPos << 17);
                }

                ulong combined =
                    (left << 8) | (left >> 8) | (right << 8) | (right >> 8) | up | down;

                knightMoveLookUp[i] = combined;
            }

            for (dynamic i = 0, sqr = (ulong)0x1; i < 64; i++, sqr <<= 1)
            {
                wPawnAttackLookUp[i] = 0x0;
                bPawnAttackLookUp[i] = 0x0;
                if (i % 8 > 0)
                {
                    wPawnAttackLookUp[i] = sqr >> 9;
                    bPawnAttackLookUp[i] = sqr << 7;
                }
                if (i % 8 < 7)
                {
                    wPawnAttackLookUp[i] |= sqr >> 7;
                    bPawnAttackLookUp[i] |= sqr << 9;
                }
            }

            for (dynamic i = 0, sqr = (ulong)0x1; i < 64; i++, sqr <<= 1)
            {
                kingMoves[i] = sqr << 8;
                kingMoves[i] |= sqr >> 8;
                if (i % 8 > 0)
                {
                    kingMoves[i] |= sqr >> 9;
                    kingMoves[i] |= sqr >> 1;
                    kingMoves[i] |= sqr << 7;
                }
                if (i % 8 < 7)
                {
                    kingMoves[i] |= sqr << 9;
                    kingMoves[i] |= sqr << 1;
                    kingMoves[i] |= sqr >> 7;
                }
            }
        }
    }
}
