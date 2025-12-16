using MH3GHDSaveEditor.SaveDataModel.Equip;
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

namespace MH3GHDSaveEditor.Pages.EquipEdit
{
    /// <summary>
    /// EquipEditPage.xaml 的交互逻辑
    /// </summary>
    public partial class EquipEditPage : Page
    {
        public EquipEditPage()
        {
            InitializeComponent();
        }

        private void EquipEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("EquipEditPage is loaded");
            EquipDataGrid.ItemsSource = null;
            EquipDataGrid.ItemsSource = MainWindow.Instance.saveDataModel.equips;
            EquipDataGrid.DataContext = MainWindow.Instance;
        }
    }
}
