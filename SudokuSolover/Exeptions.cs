using System;

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
