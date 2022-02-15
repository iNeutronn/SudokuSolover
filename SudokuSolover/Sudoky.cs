using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SudokuSolover
{
    class Sudoku
    {

        private int[,] Field;
        private Stopwatch stopwatch;
        ISudokyFileHelper sudokyHelper = new FileHelper();

        public int this[int x, int y]
        {
            get => Field[x, y];
            set => Field[x, y] = value;
        }
        private static int[,] SoloveSimplest(int[,] sudoku_Field)
        {
            bool AnyThingChenged;
            do
            {
                AnyThingChenged = false;

                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (sudoku_Field[x, y] == 0)
                        {
                            List<int> AllPossibleOptions = GetAllPossibleOptions(x, y, sudoku_Field);

                            if (AllPossibleOptions.Count == 1)//If there is only 1 possible number - write it
                            {
                                sudoku_Field[x, y] = AllPossibleOptions[0];
                                AnyThingChenged = true;
                            }
                            else if (AllPossibleOptions.Count == 0)//If there no possible number - sudoky have no slolution
                            {
                                throw new NoSolutionExeption(sudoku_Field);
                            }
                        }
                    }
                }
            } while (AnyThingChenged);

            return sudoku_Field;
        }
        public void ReadFromFile(string fileName)
        {
            Field = sudokyHelper.ReadFromFile(fileName);
        }
        public void WriteIntoFile(string fileName)
        {
            sudokyHelper.WriteIntoFile(Field, fileName);
        }

        private static List<int> GetAllPossibleOptions(int x, int y, int[,] SudokyField)
        {
            
            HashSet<int> col = GetAllUsedInCollumNumbers(x, SudokyField);
            HashSet<int> row = GetAllUsedInRowNumbers(y, SudokyField);
            HashSet<int> block = GetAllUsedInBlockNumbers(x, y, SudokyField);

            List<int> AllPossibleOptions = new();

            for (int i = 1; i < 10; i++)
                if (!col.Contains(i) && !row.Contains(i) && !block.Contains(i))
                    AllPossibleOptions.Add(i);
            return AllPossibleOptions;
        }

        private static HashSet<int> GetAllUsedInBlockNumbers(int x, int y, int[,] SudokyField)
        {
            HashSet<int> block = new();
            int sX = 3 * (x / 3);
            int sY = 3 * (y / 3);

            for (int yP = 0; yP < 3; yP++)
                for (int xP = 0; xP < 3; xP++)
                    if (SudokyField[xP + sX, yP + sY] != 0)
                        block.Add(SudokyField[xP + sX, yP + sY]);
            return block;
        }

        private static HashSet<int> GetAllUsedInRowNumbers(int y, int[,] SudokyField)
        {
            HashSet<int> row = new();
            for (int i = 0; i < 9; i++)
                if (SudokyField[i, y] != 0)
                    row.Add(SudokyField[i, y]);
            return row;
        }

        internal int[,] GetAsArray()
        {
            return Field;
        }

        private static HashSet<int> GetAllUsedInCollumNumbers(int numberOfCollum, int[,] SudokyField)
        {
            HashSet<int> col = new();
            for (int i = 0; i < 9; i++)
                if (SudokyField[numberOfCollum, i] != 0)
                    col.Add(SudokyField[numberOfCollum, i]);
            return col;
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }

        public SolutionInformation Solove(Metod m)
        {
            int[,] InputSudoky = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    InputSudoky[i, j] = Field[i, j];
                }
            }


            stopwatch = new();
            stopwatch.Start();
            switch (m)
            {
                case Metod.LastPossible:
                    Field = SoloveSimplest(InputSudoky);
                    break;
                case Metod.Recourse:
                    RecurseSolove(InputSudoky, false);
                    break;
                case Metod.Combi:
                    RecurseSolove(InputSudoky, true);
                    break;

            }
            stopwatch.Stop();

            

            SolutionInformation inf = Analyze(InputSudoky);

            return inf;

        }

        private SolutionInformation Analyze(int[,] Input_Sudoky)
        {
            SolutionInformation inf = new();
            inf.Ticks = stopwatch.ElapsedTicks;
            inf.Miliseconds = (int)stopwatch.ElapsedMilliseconds;
            inf.Fiel = Field;

            int counter = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Input_Sudoky[i, j] != 0)
                        counter++;
            inf.InitialFilling = counter / 81;

            
            int counter2 = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Field[i, j] != 0)
                        counter2++;
            inf.NumbersEntered = counter2 - counter;

            inf.IsItSoloved = counter2 == 81;
            return inf;
        }

        public void Set(int[,] sudoky)
        {
            if (!CheackSudokuСorrectness())
                throw new NoSolutionExeption(sudoky);

            Field = sudoky;
        }
        public  bool CheackSudokuСorrectness()
        {
            //Row
            for (int i = 0; i < 9; i++)
            {
                HashSet<int> Used = new();
                for (int j = 0; j < 9; j++)
                    if (Field[i, j] != 0 && Used.Contains(Field[i, j]))
                        return false;
                    else
                        Used.Add(Field[i, j]);
                Used.Clear();
            }

            //Col
            for (int i = 0; i < 9; i++)
            {
                HashSet<int> Used = new();
                for (int j = 0; j < 9; j++)
                    if (Field[j, i]!=0 && Used.Contains(Field[j, i]))
                        return false;
                    else
                        Used.Add(Field[j, i]);
                Used.Clear();
            }

            //Block
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    HashSet<int> Used = new();
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            if (Field[x * 3 + i, y * 3 + j] != 0 && Used.Contains(Field[x * 3 + i, y * 3 + j]))
                                return false;
                            else
                                Used.Add(Field[x * 3 + i, y * 3 + j]);
                    Used.Clear();

                }

            return true;
        }
        private bool RecurseSolove(int[,] Pole, bool combi)
        {
            if (combi)
                Pole = SoloveSimplest(Pole);



            for (int x = 0; x < 9; x++)
                for (int y = 0; y < 9; y++)
                    if (Pole[x, y] == 0)
                    {

                        List<int> AllPossibleOptions = GetAllPossibleOptions(x, y, Pole);
                        for (int n = 0; n < AllPossibleOptions.Count; n++)
                        {
                            Pole[x, y] = AllPossibleOptions[n];
                            try
                            {
                                int[,] temp_Pole = new int[Pole.GetLength(0), Pole.GetLength(1)];
                                for (int i = 0; i < 9; i++)
                                    for (int j = 0; j < 9; j++)
                                        temp_Pole[i, j] = Pole[i, j];
                                if (RecurseSolove(temp_Pole, combi))
                                {

                                    return true;
                                }
                            }
                            catch { }
                        }

                        return false;

                    }

            bool f = true;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Pole[i, j] == 0)
                    {
                        f = false;
                        break;
                    }
            if (f)
                Field = Pole;

            return true;

        }
    }
}