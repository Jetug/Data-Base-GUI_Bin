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
using System.IO;

namespace DataBase_GUI
{
    class FileAlreadyExistsExeption: Exception
    {
        public new string Message = "Файл уже существует";
    }

    /// <summary>
    /// Логика взаимодействия для FileSettings.xaml
    /// </summary>
    public partial class FileSettings : UserControl
    {
        public FileSettings()
        {
            InitializeComponent();
            BinFile tables = new BinFile();
            fileView.ItemsSource = tables.Load_FileList();
           
            CurrentName.Content = BinFile.fileName;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BinFile file = new BinFile();
            try
            {
                if (fileNameBox.Text == "")
                    throw new EmptyException();
                if (File.Exists(BinFile.path + fileNameBox.Text))
                    throw new FileAlreadyExistsExeption();

                file.CreateFile(fileNameBox.Text);
                fileView.ItemsSource = file.Load_FileList();
                BinFile.fileName = fileNameBox.Text;
                CurrentName.Content = BinFile.fileName;

                fileNameBox.Clear();
            }
            catch (FileAlreadyExistsExeption exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (EmptyException exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Имя файла содержит недопустимые знаки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            BinFile.fileName = (string)fileView.SelectedItem;
            CurrentName.Content = BinFile.fileName;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileView.SelectedIndex > -1)
            {
                BinFile file = new BinFile();
                string fileName = (string)fileView.SelectedItem;
                File.Delete(BinFile.path + fileName);
                CurrentName.Content = "<не выбран>";
                BinFile.fileName = null;
                fileView.ItemsSource = file.Load_FileList();
            }
        }
    }
}
