using ChessMate.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessMate
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //BitBoard.Init();
            SlidingPieceBB piece = new SlidingPieceBB();
            //KingBB pawn = new KingBB();
            //BitBoard.PrintBBRepresentation(
            //    KingBB.kingAttacks[57]);
            //SlidingPieceBB.LineAttack(0x0000, 17));
            // Console.WriteLine("{0:X16} ", BitBoard.SwapBytes(0x11010101));
            //Console.WriteLine(BitBoard.PopulationCount(BitBoard.whitePawn));
            //BitBoard.PrintBBRepresentation(0x22120A0E1222221E);
            //BitBoard.PrintBBRepresentation(BitBoard.SwapBytes(0x22120A0E1222221E));
            Console.WriteLine(BitBoardUtils.LS1BToSquarePosition(0x1800000000000000));
            Application.Run(new Form1());
        }
    }
}
