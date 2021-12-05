using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        int[,] sudoku = new int[9, 9];
        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement item in MainGrid.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).TextChanged += TextBox_TextChanged;
            }
        }
        private void UpdateView()
        {
            foreach (UIElement item in MainGrid.Children)
            {
                if (item is not TextBox)
                    continue;
                TextBox t = item as TextBox;
                t.GetValue(AutomationProperties.NameProperty);
                string[] st = t.GetValue(AutomationProperties.NameProperty).ToString().Split();
                int x = int.Parse(st[0]);
                int y = int.Parse(st[1]);
                t.Text = sudoku[y, x] == 0 ? " " : sudoku[y, x].ToString();
            }
        }
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sudoku = new int[9, 9];
                StreamReader sr = new("INPUT.TXT");
                for (int i = 0; i < 9; i++)
                {
                    string[] inp = sr.ReadLine().Split();
                    for (int j = 0; j < 9; j++)
                    {
                        int n = int.Parse(inp[j]);
                        if (n < 0 || n > 9)
                            throw new FormatException();
                        sudoku[j, i] = n;
                    }
                }
                sr.Close();

                UpdateView();

                MessageBox.Show("Судоку зчитане з файлу INPUT.TXT");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Не знайдено файл INPUT.TXT");
            }
            catch (FormatException)
            {
                MessageBox.Show("Невірний формат");
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Невірний формат");
            }
            catch
            {
                MessageBox.Show("Помилка зчитування");
            }
        }

        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter sw = new("OUTPUT.TXT");
                for (int i = 0; i < 9; i++)
                {
                    string output = "";
                    for (int j = 0; j < 9; j++)
                        output += sudoku[j, i].ToString() + " ";

                    sw.WriteLine(output);
                }
                sw.Close();
                MessageBox.Show("Судоку записане в файл OUTPUT.TXT");
            }
            catch 
            {
                MessageBox.Show("Помилка запису");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku = new int[9, 9];
            UpdateView();
        }

        private void SoloveButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Catch
            Sudoku s = new Sudoku();
            s.Set(sudoku);
            SolutionInformation solutionInformation = s.Solove(Metod.Combi);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox)
                return;
            TextBox t = sender as TextBox;
            string[] st = t.GetValue(AutomationProperties.NameProperty).ToString().Split();
            int x = int.Parse(st[0]);
            int y = int.Parse(st[1]);
            try
            {
                if (t.Text is " " or "")
                {
                    sudoku[y, x] = 0;
                }
                else
                {
                    int n = int.Parse(t.Text);
                    if (n is < 0 or > 9)
                        throw new FormatException();
                    sudoku[y, x] = n;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Невірний формат");
            }
        }
    }
}
