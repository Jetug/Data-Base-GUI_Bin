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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Threading;
using DataBaseLogic_Bin;


namespace GUI_Logic
{
    public class Logic
    {
        public void Save()
        {

        }
        
        public static void Delete<T>(DataGrid dataGrid, List<T> list)
        {
            int index = dataGrid.SelectedIndex;
            if (index > -1)
            {
                list.RemoveAt(index);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = list ;
            }
        }

        public static void Update(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = BinFile.villages;
        }

        public static void PlaySound(string soundName)
        {
            try
            {
                SoundPlayer player = new SoundPlayer($"C:/Windows/Media/{soundName}");
                player.Play();
            }
            catch
            {

            }
        }
    }
}