using System;
using System.IO;

namespace SudokuSolover
{
    class FileHelper : ISudokyFileHelper
    {
        public int[,] ReadFromFile(string fileName)
        {
            int[,] sudoku = new int[9, 9];
            StreamReader sr = new("INPUT.TXT");
            for (int i = 0; i < 9; i++)
            {
                string[] inp = sr.ReadLine().Split();
                for (int j = 0; j < 9; j++)
                {
                    int n = int.Parse(inp[j]);
                    if (n is < 0 or > 9)
                        throw new FormatException();
                    sudoku[j, i] = n;
                }
            }
            sr.Close();

            return sudoku;
        }

        public void WriteIntoFile(int[,] sudoky, string fileName)
        {
            StreamWriter sw = new("OUTPUT.TXT");
            for (int i = 0; i < 9; i++)
            {
                string output = "";
                for (int j = 0; j < 9; j++)
                    output += sudoky[j, i].ToString() + " ";

                sw.WriteLine(output);
            }
            sw.Close();
        }
    }
}
