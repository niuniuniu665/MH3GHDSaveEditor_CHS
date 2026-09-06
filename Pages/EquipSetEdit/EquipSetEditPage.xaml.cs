using MH3GHDSaveEditor.SaveDataModel.Equip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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


namespace MH3GHDSaveEditor.Pages.EquipSetEdit
{
    /// <summary>
    /// EquipSetEditPage.xaml 的交互逻辑
    /// </summary>
    public partial class EquipSetEditPage : Page
    {
        public ObservableCollection<EquipSetSlot> equipSetSlotInfo = new ObservableCollection<EquipSetSlot>(); 

        public EquipSetEditPage()
        {
            InitializeComponent();
        }

        private void EquipSetEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ItemEditPage is loaded");
            EquipSetNameDataGrid.ItemsSource = null;
            EquipSetNameDataGrid.ItemsSource = MainWindow.Instance.saveDataModel.equipSetList;
            EquipSetNameDataGrid.DataContext = MainWindow.Instance;
            equipSetSlotInfo.Clear();
            EquipSlotsDataGrid.ItemsSource = null;
            EquipSlotsDataGrid.ItemsSource = equipSetSlotInfo;
            EquipSlotsDataGrid.DataContext = this;
        }

        private void EquipSetNameDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("EquipSetNameDataGrid_SelectionChanged idx:" + EquipSetNameDataGrid.SelectedIndex.ToString());
            if (EquipSetNameDataGrid.SelectedIndex == -1)
            {
                equipSetSlotInfo.Clear();
                return;
            }

            equipSetSlotInfo.Clear();

            EquipSet equipSet = MainWindow.Instance.saveDataModel.equipSetList[EquipSetNameDataGrid.SelectedIndex];

            uint decorationIdx = 0;

            EquipSetSlot[] items = new EquipSetSlot[] {
                // 武器
                new EquipSetSlot
                {
                    EquipType = (uint)equipSet.WeaponType,
                    EquipId = equipSet.WeaponId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },
                
                // 头
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Head,
                    EquipId = equipSet.HeadId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },

                // 胴
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Body,
                    EquipId = equipSet.BodyId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },

                // 手
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Arm,
                    EquipId = equipSet.ArmId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },

                // 腰
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Waist,
                    EquipId = equipSet.WaistId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },

                // 腿
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Leg,
                    EquipId = equipSet.LegId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },
                
                // 护石
                new EquipSetSlot
                {
                    EquipType = (uint)EquipType.Talisman,
                    EquipId = equipSet.TalismanId,
                    Decoration1 = equipSet.DecorationList[decorationIdx++],
                    Decoration2 = equipSet.DecorationList[decorationIdx++],
                    Decoration3 = equipSet.DecorationList[decorationIdx++],
                },
            };

            // 按照 武器、头、胴、手、腰、腿、护石 的顺序添加
            foreach (var item in items)
            {
                equipSetSlotInfo.Add(item);
            }
        }
    }

    /// <summary>
    /// 装备套装详情
    /// </summary>
    public class EquipSetSlot : INotifyPropertyChanged
    {
        private UInt32 _equipType;
        private UInt32 _equipId;
        private UInt32 _decoration1;
        private UInt32 _decoration2;
        private UInt32 _decoration3;

        /// <summary>
        /// 装备类型
        /// </summary>
        public UInt32 EquipType { 
            get => _equipType; 
            set {
                if (_equipType != value)
                {
                    _equipType = value;
                    OnPropertyChanged();
                }
            } 
        }

        /// <summary>
        /// 装备ID
        /// </summary>
        public UInt32 EquipId
        {
            get => _equipId;
            set
            {
                if (_equipId != value)
                {
                    _equipId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 装饰珠1
        /// </summary>
        public UInt32 Decoration1
        {
            get => _decoration1;
            set
            {
                if (_decoration1 != value)
                {
                    _decoration1 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 装饰珠2
        /// </summary>
        public UInt32 Decoration2
        {
            get => _decoration2;
            set
            {
                if (_decoration2 != value)
                {
                    _decoration2 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 装饰珠3
        /// </summary>
        public UInt32 Decoration3
        {
            get => _decoration3;
            set
            {
                if (_decoration3 != value)
                {
                    _decoration3 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 装备类型名
        /// </summary>
        public string EquipTypeName 
        { 
            get
            {
                return MainWindow.Instance.EquipTypeDict.TryGetValue(_equipType, out string name) ? name : string.Empty;
            }
        }

        /// <summary>
        /// 装备名称
        /// </summary>
        public string EquipIdName
        {
            get
            {
                if (_equipId >= 0 && _equipId < SaveDataModel.SaveDataModel.ItemBoxMaxinum)
                {
                    return MainWindow.Instance.saveDataModel.equips[_equipId].EquipIdName.Name;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 装饰珠ID偏移
        /// </summary>
        public const uint DecorationIdOffset = 1111;

        /// <summary>
        /// 装饰珠1名称
        /// </summary>
        public string Decoration1Name
        {
            get
            {
                return MainWindow.Instance.ItemDict.TryGetValue(_decoration1 + DecorationIdOffset, out string name) == true ? name : string.Empty;
            }
        }

        /// <summary>
        /// 装饰珠2名称
        /// </summary>
        public string Decoration2Name
        {
            get
            {
                return MainWindow.Instance.ItemDict.TryGetValue(_decoration2 + DecorationIdOffset, out string name) == true ? name : string.Empty;
            }
        }

        /// <summary>
        /// 装饰珠3名称
        /// </summary>
        public string Decoration3Name
        {
            get
            {
                return MainWindow.Instance.ItemDict.TryGetValue(_decoration3 + DecorationIdOffset, out string name) == true ? name : string.Empty;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
