using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataBaseLogic_Bin;
using GUI_Logic;

namespace DataBase_GUI
{
    /// <summary>
    /// Логика взаимодействия для DeveloperTable.xaml
    /// </summary>
    public partial class DeveloperTable : UserControl
    {
        public DeveloperTable()
        {
            InitializeComponent();
            DeveloperData.ItemsSource = BinFile.developers;
        }
        private BinFile file = new BinFile();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Developer dev = new Developer();
            try
            {
                if ((nameTextBox.Text.Trim() == "") || (addrTextBox.Text.Trim() == ""))
                {
                    throw new EmptyException();
                }
                dev.name = nameTextBox.Text.Trim();
                dev.inc = float.Parse(incTextBox.Text);
                dev.addr = addrTextBox.Text.Trim();

                nameTextBox.Clear();
                incTextBox.Clear();
                addrTextBox.Clear();

                BinFile.developers.Add(dev);
                DeveloperData.ItemsSource = null;
                DeveloperData.ItemsSource = BinFile.developers;
            }
            catch (OverflowException)
            {
                Logic.PlaySound("Windows Ding.wav");
                MessageBox.Show("Было введено слишком малое или слишком большое значение", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException exc)
            {
                Logic.PlaySound("Windows Ding.wav");
                MessageBox.Show(exc.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (EmptyException exc)
            {
                Logic.PlaySound("Windows Ding.wav");
                MessageBox.Show(exc.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            file.SaveAll();
            this.Cursor = Cursors.Arrow;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            DeveloperData.ItemsSource = null;
            DeveloperData.ItemsSource = BinFile.developers;
            Cursor = Cursors.Arrow;
        }

        SearchBox search;

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            search = new SearchBox();
            search.FindButton.Click += FindButton_Click;

            search.SearchedElement.ItemsSource = new List<string> { "Название" , "Годовой доход", "Адрес" };
            search.ShowDialog();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            List<Developer> sortedList = BinFile.Search(BinFile.developers, search.SearchedValue.Text, search.SearchedElement.SelectedIndex);

            if (sortedList.Count > 0)
            {
                DeveloperData.ItemsSource = null;
                DeveloperData.ItemsSource = sortedList;
            }
            else
            {
                MessageBox.Show("Ничего не найдено", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.Delete(DeveloperData, BinFile.developers); 
        }

        private void DeveloperData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox editedTextbox = e.EditingElement as TextBox;

            Developer dev = BinFile.developers[DeveloperData.SelectedIndex];
            try
            {
                if (DeveloperData.CurrentColumn != null)
                {
                    switch (DeveloperData.CurrentColumn.DisplayIndex)
                    {
                        case 0:
                            dev.name = editedTextbox.Text;
                            break;
                        case 1:
                            dev.inc = float.Parse(editedTextbox.Text);
                            break;
                        case 2:
                            dev.addr = editedTextbox.Text;
                            break;
                    }
                    BinFile.developers[DeveloperData.SelectedIndex] = dev;
                }
                DeveloperData.ItemsSource = null;
                DeveloperData.ItemsSource = BinFile.developers;
            }
            catch (FormatException) { }
            catch (OverflowException exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
