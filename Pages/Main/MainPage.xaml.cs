using MH3GHDSaveEditor.Common;
using MH3GHDSaveEditor.SaveDataModel.Equip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace MH3GHDSaveEditor.Pages.Main
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            this.DataContext = MainWindow.Instance.saveDataModel;
        }

        /// <summary>
        /// 打开存档
        /// </summary>
        public void OpenData()
        {
            string filePath = FileManager.ChooseFile();
            if (File.Exists(filePath))
            {
                byte[] data = File.ReadAllBytes(filePath);

                if (data != null)
                {
                    if (data.Length != MainWindow.Instance.saveDataModel.AddressOffset.SaveDataSize)
                    {
                        MessageBox.Show("存档大小不匹配！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MainWindow.Instance.saveDataModel.SaveData = data;
                        MainWindow.Instance.IsReadSaveData = true;
                        MainWindow.Instance.readSaveDataPath = filePath;
                    }
                }
            }
        }

        public static void CopySaveData(string srcFilePath)
        {
            string destFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "backup", 
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (!Directory.Exists(destFilePath))
            {
                Directory.CreateDirectory(destFilePath);
            }
            destFilePath = System.IO.Path.Combine(destFilePath, System.IO.Path.GetFileName(srcFilePath));
            try
            {
                File.Copy(srcFilePath, destFilePath, overwrite: true);
                MainWindow.Instance.SendMessage("已自动备份存档");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MainWindow.Instance.SendMessage("自动备份存档失败");
            }
        }

        /// <summary>
        /// 关闭并保存存档
        /// </summary>
        public static void CloseAndSaveData()
        {
            if (MainWindow.Instance.IsReadSaveData)
            {
                if (Properties.Settings.Default.IsAutoBackup)
                {
                    MainPage.CopySaveData(MainWindow.Instance.readSaveDataPath);
                }
                FileManager.CreateFile(MainWindow.Instance.readSaveDataPath);
                MainWindow.Instance.saveDataModel.Save2DataBytes();
                File.WriteAllBytes(MainWindow.Instance.readSaveDataPath, MainWindow.Instance.saveDataModel.SaveData);
                MainWindow.Instance.saveDataModel.SaveData = null;
                MainWindow.Instance.IsReadSaveData = false;
                MainWindow.Instance.readSaveDataPath = null;

                MainWindow.Instance.SendMessage("保存成功");
            }
            else
            {
                MainWindow.Instance.SendMessage("没有打开的存档");
            }
        }
    }
}
