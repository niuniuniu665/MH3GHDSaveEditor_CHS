using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MH3GHDSaveEditor.Pages.ItemEdit
{
    /// <summary>
    /// ItemEditPage.xaml 的交互逻辑
    /// </summary>
    public partial class ItemEditPage : Page
    {
        public ItemEditPage()
        {
            InitializeComponent();
        }

        private void ItemEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ItemEditPage is loaded");
            ItemDataGrid.ItemsSource = null;
            ItemDataGrid.ItemsSource = MainWindow.Instance.saveDataModel.items;
            ItemDataGrid.DataContext = MainWindow.Instance;
        }
    }
}
