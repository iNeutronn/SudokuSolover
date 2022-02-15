using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolover
{
    interface ISudokyFileHelper
    {
        int[,] ReadFromFile(string fileName);
        void WriteIntoFile(int[,] sudoky, string fileName);
    }
}
