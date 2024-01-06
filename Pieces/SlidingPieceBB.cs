namespace ChessMate.Pieces
{
    internal class SlidingPieceBB
    {
        public SlidingPieceBB() 
        {
            for (int sq = 0; sq < 64; sq++)
            {
                smsk[sq].diagonalMaskEx = diagonalMask(sq);
                smsk[sq].antidiagMaskEx = antiDiagMask(sq);
                smsk[sq].fileMaskEx = fileMask(sq);
                smsk[sq].rankMaskEx = rankMask(sq);
                smsk[sq].bitMask = (ulong)(0x1 << sq);
            }
        }

        public static ulong diagonalMask(int sq)
        {
            const ulong maindia = 0x8040201008040201;
            int diag = 8 * (sq & 7) - (sq & 56);
            int nort = -diag & (diag >> 31);
            int sout = diag & (-diag >> 31);
            return (maindia >> sout) << nort;
        }

        public static ulong antiDiagMask(int sq)
        {
            const ulong maindia = 0x0102040810204080;
            int diag = 56 - 8 * (sq & 7) - (sq & 56);
            int nort = -diag & (diag >> 31);
            int sout = diag & (-diag >> 31);
            return (maindia >> sout) << nort;
        }

        public static ulong rankMask(int sq) { return (ulong)(0xff << (sq & 56)); }

        public static ulong fileMask(int sq) { return (ulong)(0x0101010101010101 << (sq & 7)); }

        public static ulong getPossibleMoves(ulong occupancy, ulong piece)
        {
            return occupancy - 2 * piece;
        }

        public struct smask
        {
            public ulong bitMask;         // 1 << sq for convenience
            public ulong diagonalMaskEx;
            public ulong antidiagMaskEx;
            public ulong fileMaskEx;
            public ulong rankMaskEx;
        }

        /// <summary>
        /// Lookup table for each mask
        /// </summary>
        public static smask[] smsk = new smask[64]; // 2.5 KBytes

        public static ulong LineAttack(ulong o, int sq)
        {
            ulong r = (ulong)0x1 << sq;
            return (((smsk[sq].rankMaskEx & o) - 2 * r) ^ 
                BitBoardUtils.SwapBytes(BitBoardUtils.SwapBytes(o) - 2 * BitBoardUtils.SwapBytes(r)))
                & smsk[sq].rankMaskEx;
        }

        /// <summary>
        /// Hyperbola Quintessence algorithm for generating sliding piece attacks
        /// </summary>
        /// <param name="occupancy">occupancy bitboard excluding the piece itself</param>
        /// <param name="position">piece position bitboard</param>
        /// <param name="mask">diagonal, antidiagonal, file, rank masks, located in smask lookup table</param>
        /// <returns>possible moves of sliding piece and direction of slide</returns>
        public static ulong HyperbolaQuintessence(ulong occupancy, ulong position, ulong mask)
        {
            ulong forward, reverse;
            forward = occupancy & mask;
            reverse = BitBoardUtils.SwapBytes(forward);
            forward -= position;
            reverse -= BitBoardUtils.SwapBytes(position);
            forward ^= BitBoardUtils.SwapBytes(reverse);
            forward &= mask;
            return forward;
        }

        // Hyperbola Quintessence
        public static ulong diagonalAttacks(ulong occ, int sq)
        {
            ulong forward, reverse;
            forward = occ & smsk[sq].diagonalMaskEx;
            reverse = BitBoardUtils.SwapBytes(forward);
            forward -= smsk[sq].bitMask;
            reverse -= BitBoardUtils.SwapBytes(smsk[sq].bitMask);
            forward ^= BitBoardUtils.SwapBytes(reverse);
            forward &= smsk[sq].diagonalMaskEx;
            return forward;
        }

        ulong antiDiagAttacks(ulong occ, int sq)
        {
            ulong forward, reverse;
            forward = occ & smsk[sq].antidiagMaskEx;
            reverse = BitBoardUtils.SwapBytes(forward);
            forward -= smsk[sq].bitMask;
            reverse -= BitBoardUtils.SwapBytes(smsk[sq].bitMask);
            forward ^= BitBoardUtils.SwapBytes(reverse);
            forward &= smsk[sq].antidiagMaskEx;
            return forward;
        }

        public static ulong fileAttacks(ulong occ, int sq)
        {
            ulong forward, reverse;
            forward = occ & smsk[sq].fileMaskEx;
            reverse = BitBoardUtils.SwapBytes(forward);
            forward -= smsk[sq].bitMask;
            reverse -= BitBoardUtils.SwapBytes(smsk[sq].bitMask);
            forward ^= BitBoardUtils.SwapBytes(reverse);
            forward &= smsk[sq].fileMaskEx;
            return forward;
        }
    }
}
