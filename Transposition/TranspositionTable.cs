using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Transposition
{
	public class TranspositionTable<T> where T : ITransposable
	{
		public Dictionary<long, int> Table { get; set; } = new Dictionary<long, int>();

		int Hits { get; set; } = 0;
		int Total { get; set; } = 0;

		public int Evaluate(T board)
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
