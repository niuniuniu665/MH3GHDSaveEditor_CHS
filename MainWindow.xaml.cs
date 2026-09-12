using MaterialDesignThemes.Wpf;
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

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

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

            SystemParameters.StaticPropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SystemParameters.HighContrast) ||
                    e.PropertyName == "WindowGlassColor") // 系统主题相关
                {
                    UpdateDataGridBrushes();
                }
            };

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
        private void UpdatePrimaryColorCheckState(Color color)
        {
            foreach (var obj in PrimaryColorMenu.Items)
            {
                if (obj is MenuItem item && item.Tag is string hex)
                {
                    var c = (Color)ColorConverter.ConvertFromString(hex);
                    item.IsChecked = (c == color);
                }
            }
        }
        private void UpdateDataGridBrushes()
        {
            var theme = _paletteHelper.GetTheme();
            bool isDark = theme.GetBaseTheme() == BaseTheme.Dark;

            // 深色主题：交替行比背景略亮一点
            // 浅色主题：交替行比背景略暗一点
            var altColor = isDark
                ? Color.FromRgb(0x32, 0x32, 0x32)
                : Color.FromRgb(0xF5, 0xF5, 0xF5);

            var normalColor = isDark
                ? Color.FromRgb(0x1E, 0x1E, 0x1E)
                : Colors.White;

            Application.Current.Resources["DataGridRowBackgroundBrush"] =
                new SolidColorBrush(normalColor);

            Application.Current.Resources["DataGridAlternatingRowBackgroundBrush"] =
                new SolidColorBrush(altColor);
        }

        private void ApplyPrimaryColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();
            theme.SetPrimaryColor(color);
            _paletteHelper.SetTheme(theme);

            UpdatePrimaryColorCheckState(color);

            UpdateDataGridBrushes();

            // 持久化
            Properties.Settings.Default.PrimaryColor = color.ToString();
            Properties.Settings.Default.Save();
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
