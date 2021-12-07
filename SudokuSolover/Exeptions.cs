using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolover
{
    class NoSolutionExeption : Exception
    {
        public int[,] SudokyField
        {
            get;
            private set;
        }
        public NoSolutionExeption(int[,] s)
        {
            SudokyField = s;
        }
    }
}
