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
using System.Windows.Shapes;

namespace DataBase_GUI
{
    /// <summary>
    /// Логика взаимодействия для SearchBox.xaml
    /// </summary>
    public partial class SearchBox : Window
    {
        public SearchBox()
        {
            InitializeComponent();
            SearchedValue.IsEnabled = false;
        }

        public List<object> list = new List<object>();

        private void SearchedElement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchedValue.IsEnabled = true;
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
