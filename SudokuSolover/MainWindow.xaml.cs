using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
                if (item is TextBox box)
                    box.TextChanged += TextBox_TextChanged;
            }
            MetodComboBox.SelectedIndex = 1;
            LeftInfLabel.Content = "Вирішене: - \n Мілісекунди: - \n Тіки: -";
        }
        private void UpdateView()
        {
            foreach (UIElement item in MainGrid.Children)
            {
                if (item is not TextBox)
                    continue;
                try
                {
                    TextBox t = item as TextBox;
                    t.GetValue(AutomationProperties.NameProperty);
                    string[] st = t.GetValue(AutomationProperties.NameProperty).ToString().Split();
                    int x = int.Parse(st[0]);
                    int y = int.Parse(st[1]);
                    t.Text = sudoku[y, x] == 0 ? "" : sudoku[y, x].ToString();
                }
                catch(FormatException)
                {
                    
                }
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
                        if (n is < 0 or > 9)
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
                MessageBox.Show("Не знайдено файл INPUT.TXT","Помилка!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Невірний формат", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Невірний формат", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("Помилка зчитування", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Помилка запису", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku = new int[9, 9];
            UpdateView();
        }

        
        private void SoloveButton_Click(object sender, RoutedEventArgs e)
        {

            if (MetodComboBox.SelectedIndex == 0)
                MessageBox.Show("Спосіб останнього можливого найшвидший, та в деяких випадках він безсилий. \n Найоптимальнішив варіантом буде рекурсивний спосіб.", "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);


            Sudoku s = new();
            s.Set(sudoku);
           
            SolutionInformation inf = MetodComboBox.SelectedIndex switch
            {
                0 => s.Solove(Metod.LastPossible),
                1 => s.Solove(Metod.Recourse),
                2 => s.Solove(Metod.Combi),
                _ => throw new Exception(),
            };
            LeftInfLabel.Content = "Вирішене: " + (inf.IsItSoloved ? "Так" : "Ні") + "\n";
            LeftInfLabel.Content += "Мілісекунди:" + inf.Miliseconds + "\n";
            LeftInfLabel.Content += "Тіки:" + inf.Ticks;

            

            sudoku = inf.Fiel;
            UpdateView();


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
                MessageBox.Show("Невірний формат", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
