using MH3GHDSaveEditor.Pages.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MH3GHDSaveEditor
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private void AutoBackupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                Properties.Settings.Default.IsAutoBackup = (bool)checkBox.IsChecked;
                Properties.Settings.Default.Save();
            }
        }

        private void AutoBackupCheckBox_Initialized(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                checkBox.IsChecked = Properties.Settings.Default.IsAutoBackup;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (IsReadSaveData)
            {
                var result = MessageBox.Show("是否保存所做的修改?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    MainPage.CloseAndSaveData();
                }
            }
        }
    }
}
