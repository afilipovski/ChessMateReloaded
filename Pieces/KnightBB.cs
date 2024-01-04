namespace ChessMate.Pieces
{
    public class KnightBB
    {
        public static ulong blackKnight = 0x0000000000000024;
        public static ulong whiteKnight = 0x2400000000000000;

        public static ulong[] knightAttackLookUp = new ulong[64];

        public KnightBB()
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

                knightAttackLookUp[i] = combined;
            }
        }
    }
}
