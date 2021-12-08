using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SudokuSolover
{
    class Sudoku
    {

        int[,] Input_Sudoky;
        int[,] Output_Sudoku;
        private Stopwatch sw;
        
        private static int[,] SoloveSimplest(int[,] sudoku_Field)
        {
            bool f = true;//перевіряєм чи алгоритм вніс хоч якісь зміни, якщо ні то далі він уже безсилий
            while (f)
            {
                f = false;

                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (sudoku_Field[x, y] == 0)
                        {//формуємо набори чисел які вже не можуть бути в цій клітинці тому що вони вже є 
                            List<int> AllPossibleOptions = GetAllPossibleOptions(x, y, sudoku_Field);

                            if (AllPossibleOptions.Count == 1)//якщо можливе лише 1 число то впичуєш його 
                            {
                                sudoku_Field[x, y] = AllPossibleOptions[0];
                                f = true;
                            }
                            else if (AllPossibleOptions.Count == 0)
                            {
                                throw new NoSolutionExeption(sudoku_Field);
                            }
                        }
                    }
                }
            }
            return sudoku_Field;
        }

        
        private static List<int> GetAllPossibleOptions(int x, int y, int[,] P)
        {
            HashSet<int> col = new();
            for (int i = 0; i < 9; i++)
                if (P[x, i] != 0)
                    col.Add(P[x, i]);

            HashSet<int> row = new();
            for (int i = 0; i < 9; i++)
                if (P[i, y] != 0)
                    row.Add(P[i, y]);


            HashSet<int> block = new();
            int sX = 3 * (x / 3);
            int sY = 3 * (y / 3);

            for (int yP = 0; yP < 3; yP++)
                for (int xP = 0; xP < 3; xP++)
                    if (P[xP + sX, yP + sY] != 0)
                        block.Add(P[xP + sX, yP + sY]);




            List<int> AllPossibleOptions = new();
            for (int i = 1; i < 10; i++)
                if (!col.Contains(i) && !row.Contains(i) && !block.Contains(i))
                    AllPossibleOptions.Add(i);
            return AllPossibleOptions;
        }

        public SolutionInformation Solove(Metod m)
        {
            sw = new();
            sw.Start();
            switch (m)
            {
                case Metod.LastPossible:
                    Output_Sudoku = SoloveSimplest(Input_Sudoky);
                    break;
                case Metod.Recourse:
                    RecurseSolove(Input_Sudoky, false);
                    break;
                case Metod.Combi:
                    RecurseSolove(Input_Sudoky, true);
                    break;

            }
            sw.Stop();

            if (Output_Sudoku == null)
                Output_Sudoku = SoloveSimplest(Input_Sudoky);

            SolutionInformation inf = new();
            inf.Ticks = sw.ElapsedTicks;
            inf.Miliseconds = (int)sw.ElapsedMilliseconds;
            inf.Fiel = Output_Sudoku;

            int counter = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Input_Sudoky[i, j] != 0)
                        counter++;
            inf.InitialFilling = counter / 81;

            if (Output_Sudoku == null)
                Output_Sudoku = SoloveSimplest(Input_Sudoky);
            int counter2 = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Output_Sudoku[i, j] != 0)
                        counter2++;
            inf.NumbersEntered = counter2 - counter;

            inf.IsItSoloved = counter2 == 81;

            return inf;

        }
        public void Set(int[,] sudoky)
        {
           
            if (!CheackSudokuСorrectness(sudoky))
                throw new NoSolutionExeption(sudoky);
            Input_Sudoky = sudoky;
        }
        private static bool CheackSudokuСorrectness(int[,] inp)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (inp[i,j] == 0 && GetAllPossibleOptions(i, j,inp).Count == 0)
                        return false;
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
                Output_Sudoku = Pole;

            return true;

        }
    }
}