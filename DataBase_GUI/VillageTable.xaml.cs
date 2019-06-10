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
using System.Media;
using System.Threading;
using DataBaseLogic_Bin;
using GUI_Logic;

namespace DataBase_GUI
{
    /// <summary>
    /// Логика взаимодействия для DataTable.xaml
    /// </summary>
    public partial class VillageTable : UserControl
    {
        public VillageTable()
        {
            InitializeComponent();
            VillageData.ItemsSource = BinFile.villages;
            devComboBox.ItemsSource = file.GetDevNames();
        }

        private BinFile file = new BinFile();
        private bool needToSave = true;

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Village vill = new Village();
            try
            {
                if ((nameTextBox.Text.Trim() == "") || (devComboBox.Text.Trim() == ""))
                {
                    throw new EmptyException();
                }
                vill.name = nameTextBox.Text.Trim();
                vill.dev = devComboBox.Text.Trim();
                vill.area = float.Parse(areaTextBox.Text);
                vill.people = uint.Parse(peopleTextBox.Text);

                nameTextBox.Clear();
                devComboBox.SelectedIndex = -1;
                areaTextBox.Clear();
                peopleTextBox.Clear();

                BinFile.villages.Add(vill);
                VillageData.ItemsSource = null;
                VillageData.ItemsSource = BinFile.villages;
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
            Cursor = Cursors.Wait;
            //needToSave = false;
            file.SaveAll();
            Cursor = Cursors.Arrow;
        }


        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            VillageData.ItemsSource = null;
            VillageData.ItemsSource = BinFile.villages;
            Cursor = Cursors.Arrow;
        }

        SearchBox search;

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            List<Village> sortedList = BinFile.Search(BinFile.villages, search.SearchedValue.Text, search.SearchedElement.SelectedIndex);

            if (sortedList.Count > 0)
            {
                VillageData.ItemsSource = null;
                VillageData.ItemsSource = sortedList;
            }
            else
            {
                MessageBox.Show("Ничего не найдено", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            search = new SearchBox();
            search.FindButton.Click += FindButton_Click;
            
            search.SearchedElement.ItemsSource = new List<string>{"Название","Девелопер","Площадь","Население"};
            search.ShowDialog();
        }

        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.Delete(VillageData ,BinFile.villages);
        }

        private void VillageData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox editedTextbox = e.EditingElement as TextBox;
            
            Village vill = BinFile.villages[VillageData.SelectedIndex];
            try
            {
                if (VillageData.CurrentColumn != null)
                {
                    switch (VillageData.CurrentColumn.DisplayIndex)
                    {
                        case 0:
                            vill.name = editedTextbox.Text;
                            break;
                        case 1:
                            vill.dev = editedTextbox.Text;
                            break;
                        case 2:
                            vill.area = float.Parse(editedTextbox.Text);
                            break;
                        case 3:
                            vill.people = uint.Parse(editedTextbox.Text);
                            break;
                    }
                    BinFile.villages[VillageData.SelectedIndex] = vill;
                }
                VillageData.ItemsSource = null;
                VillageData.ItemsSource = BinFile.villages;
            }
            catch (FormatException)
            {

            }
        }                                  
    }
}