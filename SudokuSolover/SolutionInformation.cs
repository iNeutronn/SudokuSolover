namespace SudokuSolover
{
    enum Metod
    {
        LastPossible,
        Recourse,
        Combi
    }
    struct SolutionInformation
    {
        public int[,] Fiel;
        public int Miliseconds;
        public long Ticks;
        public bool IsItSoloved;
        public double InitialFilling;
        public int NumbersEntered;
    }
}
