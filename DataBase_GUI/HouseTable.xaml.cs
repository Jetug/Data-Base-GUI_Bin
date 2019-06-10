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
    /// Логика взаимодействия для HouseTable.xaml
    /// </summary>
    public partial class HouseTable : UserControl
    {
        public HouseTable()
        {
            InitializeComponent();
            HouseData.ItemsSource = BinFile.houses;
        }

        private BinFile file = new BinFile();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            House house = new House();
            try
            {
                if ((nameTextBox.Text.Trim() == "") || (typeTextBox.Text.Trim() == ""))
                {
                    throw new EmptyException();
                }
                house.name = nameTextBox.Text.Trim();
                house.num = ushort.Parse(numTextBox.Text);
                house.area = float.Parse(areaTextBox.Text);
                house.floor = byte.Parse(floorTextBox.Text);
                house.type = typeTextBox.Text.Trim();

                nameTextBox.Clear();
                numTextBox.Clear();
                areaTextBox.Clear();
                floorTextBox.Clear();
                typeTextBox.Clear();

                BinFile.houses.Add(house);
                HouseData.ItemsSource = null;
                HouseData.ItemsSource = BinFile.houses;
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
            HouseData.ItemsSource = null;
            HouseData.ItemsSource = BinFile.houses;
            Cursor = Cursors.Arrow;
        }

        SearchBox search;

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            search = new SearchBox();
            search.FindButton.Click += FindButton_Click;

            search.SearchedElement.ItemsSource = new List<string> {"Название посёлка", "Номер" , "Площадь ", "Количество этажей", "Тип дома"};
            search.ShowDialog();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            List<House> sortedList = BinFile.Search(BinFile.houses, search.SearchedValue.Text, search.SearchedElement.SelectedIndex);

            if (sortedList.Count > 0)
            {
                HouseData.ItemsSource = null;
                HouseData.ItemsSource = sortedList;
            }
            else
            {
                MessageBox.Show("Ничего не найдено", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.Delete(HouseData, BinFile.houses);
        }

        private void HouseData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox editedTextbox = e.EditingElement as TextBox;

            House hs = BinFile.houses[HouseData.SelectedIndex];
            try
            {
                if (HouseData.CurrentColumn != null)
                {
                    switch (HouseData.CurrentColumn.DisplayIndex)
                    {
                        case 0:
                            hs.name = editedTextbox.Text;
                            break;
                        case 1:
                            hs.num = ushort.Parse(editedTextbox.Text);
                            break;
                        case 2:
                            hs.area = float.Parse(editedTextbox.Text);
                            break;
                        case 3:
                            hs.floor = byte.Parse(editedTextbox.Text);
                            break;
                        case 4:
                            hs.type = editedTextbox.Text;
                            break;
                    }
                    BinFile.houses[HouseData.SelectedIndex] = hs;
                }
                HouseData.ItemsSource = null;
                HouseData.ItemsSource = BinFile.houses;
            }
            catch (FormatException){ }
        }
    }
}
