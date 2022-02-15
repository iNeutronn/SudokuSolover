using System;
using System.IO;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace SudokuSolover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        
        Sudoku sudoku = new Sudoku();
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
                catch (FormatException)
                {

                }
            }
        }
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                sudoku.ReadFromFile("INPUT.TXT");//TODO: File Selection
                UpdateView();
                MessageBox.Show("Судоку зчитане з файлу INPUT.TXT");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Не знайдено файл INPUT.TXT", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                sudoku.WriteIntoFile("INPUT.TXT");
                MessageBox.Show("Судоку записане в файл OUTPUT.TXT");
            }
            catch
            {
                MessageBox.Show("Помилка запису", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            
            sudoku.Clear();
            
            UpdateView();
        }


        private void SoloveButton_Click(object sender, RoutedEventArgs e)
        {

            if (MetodComboBox.SelectedIndex == 0)
                MessageBox.Show("Спосіб останнього можливого найшвидший, та в деяких випадках він безсилий. \n Найоптимальнішив варіантом буде рекурсивний спосіб.", "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (MetodComboBox.SelectedIndex == 2)
                MessageBox.Show("Комбінативний спосіб вимагає виклик способа останнього можливого після кожної інерації рекурсії.", "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);

            if (!sudoku.CheackSudokuСorrectness())
            {
                MessageBox.Show("Невірний формат", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            SolutionInformation inf = MetodComboBox.SelectedIndex switch
            {
                0 => sudoku.Solove(Metod.LastPossible),
                1 => sudoku.Solove(Metod.Recourse),
                2 => sudoku.Solove(Metod.Combi),
                _ => throw new Exception(),
            };
            LeftInfLabel.Content = "Вирішене: " + (inf.IsItSoloved ? "Так" : "Ні") + "\n";
            LeftInfLabel.Content += "Мілісекунди:" + inf.Miliseconds + "\n";
            LeftInfLabel.Content += "Тіки:" + inf.Ticks;



            
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
