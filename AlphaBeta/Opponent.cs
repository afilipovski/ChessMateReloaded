using ChessMate.Pieces;
using ChessMate.Transposition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessMate.AlphaBeta
{
    public enum OpponentDifficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2,
        ITERATIVE_DEEPENING = 3
    }

    [Serializable]
    public class Opponent
    {
        static readonly Random r = new Random();

        public Opponent(OpponentDifficulty difficulty)
        {
            Difficulty = difficulty;
        }

        public OpponentDifficulty Difficulty { get; set; }

        private struct Node
        {
            public Board board;
            public int value;

            public Node(Board board, int value)
            {
                this.board = board;
                this.value = value;
            }
        }

        public Board BestMovePly(Board board, int ply)
        {
			List<Node> nodes = new List<Node>();
			int pivot_value = board.WhiteTurn ? -EvaluationUtils.INFTY : EvaluationUtils.INFTY;
			foreach (Board move in board.Successor())
			{
				int value = EvaluationUtils.AlphabetaInit(move, ply, board.WhiteTurn);
				pivot_value = board.WhiteTurn ? Math.Max(pivot_value, value) : Math.Min(pivot_value, value);
				nodes.Add(new Node(move, value));
			}
			List<Node> eligibleMoves = nodes.FindAll(n => n.value == pivot_value);
			if (eligibleMoves.Count > 0)
			{
				Board next = eligibleMoves[r.Next(eligibleMoves.Count)].board;
				Position newPos = next.NewPos;
				return next;
			}
			return null; //returns null if there are no possible moves.
		}

		public Board FixedDepthMove(Board board)
		{
			return BestMovePly(board, (int)Difficulty);
		}

		public Board IterativeDeepeningSearch(Board root, TimeSpan timeSpan)
        {
            Board best = null;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; stopwatch.Elapsed < timeSpan && i <= (int)OpponentDifficulty.MEDIUM; i++)
            {
                best = BestMovePly(root, i);
            }

            return best;
        }
    }
}
