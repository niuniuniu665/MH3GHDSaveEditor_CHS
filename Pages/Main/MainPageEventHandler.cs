using MH3GHDSaveEditor.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MH3GHDSaveEditor.Pages.Main
{
    public partial class MainPage : Page
    {
        private void OpenSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.IsReadSaveData)
            {
                var res = MessageBox.Show("已有打开的存档，要打开其他存档吗?\n是：保存所有修改并关闭存档\n否：不保存修改\n取消：返回，不进行操作", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    CloseAndSaveData();
                    OpenData();
                }
                else if (res == MessageBoxResult.No)
                {
                    OpenData();
                }
            }
            else
            {
                OpenData();
            }
        }

        private void CloseSaveFile_Click(object sender, RoutedEventArgs e)
        {
            CloseAndSaveData();
        }
    }
}
