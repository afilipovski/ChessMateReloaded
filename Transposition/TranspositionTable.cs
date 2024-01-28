using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Transposition
{
	public static class TranspositionTable<T> where T : ITransposable
	{
		public static Dictionary<long, int> Table { get; set; } = new Dictionary<long, int>();

		static int Hits { get; set; } = 0;
		static int Total { get; set; } = 0;

		public static int Evaluate(T board)
		{
			long hash = board.Hash();
			Total++;
			if (Table.ContainsKey(hash))
			{
				Hits++;
				return Table[hash];
			}
			int evaluation = board.Evaluation();
			Table.Add(hash, evaluation);
			return evaluation;
		} 
	}
}
