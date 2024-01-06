using ChessMate.AlphaBeta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate
{
    public class GameStateBB
    {
        public BitBoard Board { get; set; }
        public List<BitBoard> SuccessiveBoards { get; set; } = new List<BitBoard>();
        public Opponent o { get; set; }
        public ColoredPosition checkPosition { get; set; } = null;
        public GameStateBB()
        {
            Board = new BitBoard();
            SuccessiveBoards = new List<BitBoard>();
            o = new Opponent(OpponentDifficulty.EASY);
            checkPosition = null;
        }

        /*public void Draw(Graphics g)
        {
            Board.DrawTiles(g);
            foreach (Board sb in successiveBoards)
            {
                sb.NewPos.Draw(g);
            }
            if (checkPosition is ColoredPosition)
            {
                checkPosition.Draw(g);
            }
        }

        public void SetCheckPosition()
        {
            checkPosition = null;
            Position king = Board.KingCheckPosition(Board.WhiteTurn);
            if (king is null)
                return;
            checkPosition = new ColoredPosition(king, PositionColor.Red);
        }*/
    }
}
