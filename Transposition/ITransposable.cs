using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMate.Transposition
{
	public interface ITransposable
	{
		long Hash();
		int Evaluation();
	}
}
