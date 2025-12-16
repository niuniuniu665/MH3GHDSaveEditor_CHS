using MH3GHDSaveEditor.Common;
using MH3GHDSaveEditor.SaveDataModel;
using MH3GHDSaveEditor.SaveDataModel.Equip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MH3GHDSaveEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// 当前主窗口实例
        /// </summary>
        public static MainWindow Instance { get; private set; }

        /// <summary>
        /// 标题文本，建立属性用于绑定
        /// </summary>
        public string PageTitle { get; set; } = string.Empty;

        private bool _isReadSaveData = false;
        /// <summary>
        /// 是否已经读取了存档
        /// </summary>
        public bool IsReadSaveData 
        {
            get => _isReadSaveData; 
            set
            {
                if (_isReadSaveData != value)
                {
                    _isReadSaveData = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 已读取的存档路径
        /// </summary>
        public string readSaveDataPath;

        /// <summary>
        /// 存档信息
        /// </summary>
        public SaveDataModel.SaveDataModel saveDataModel = new SaveDataModel.SaveDataModel();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            this.DataContext = this;

            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "configs", "MH3GHD.json");
            LoadJsonConfigAsync(path);

            LoadEquipIdName();
        }

        /// <summary>
        /// 消息弹窗
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                snackbar.MessageQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path"></param>
        public void LoadJsonConfigAsync(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            Task.Run(() =>
            {
                string jsonText = File.ReadAllText(path);
                saveDataModel = new SaveDataModel.SaveDataModel
                {
                    AddressOffset = JsonConvert.DeserializeObject<AddressOffset>(jsonText)
                };
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
