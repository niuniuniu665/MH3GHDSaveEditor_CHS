using MH3GHDSaveEditor.SaveDataModel;
using MH3GHDSaveEditor.SaveDataModel.Equip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MH3GHDSaveEditor
{
    /// <summary>
    /// 加载所有装备的Id与Name对
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Dictionary<UInt32, string>[] EquipDict { get; set; } = new Dictionary<UInt32, string>[(int)EquipType.Count];
        private ObservableCollection<IdNamePair>[] equipList { get; set; } = new ObservableCollection<IdNamePair>[(int)EquipType.Count];
        public ObservableCollection<IdNamePair>[] EquipList { get => equipList; }

        /// <summary>
        /// 技能字典
        /// </summary>
        public Dictionary<UInt32, string> SkillDict;
        private ObservableCollection<IdNamePair> skillList;
        public ObservableCollection<IdNamePair> SkillList { get => skillList; }

        /// <summary>
        /// 道具字典
        /// </summary>
        public Dictionary<UInt32, string> ItemDict;
        public ObservableCollection<IdNamePair> itemList;
        public ObservableCollection<IdNamePair> ItemList { get => itemList; }

        /// <summary>
        /// 装饰珠字典
        /// </summary>
        public Dictionary<UInt32, string> DecorationDict;
        public ObservableCollection<IdNamePair> decorationList;
        public ObservableCollection<IdNamePair> DecorationList { get => decorationList; }

        /// <summary>
        /// 装备类型字典
        /// </summary>
        public Dictionary<UInt32, string> EquipTypeDict;
        public ObservableCollection<IdNamePair> equipTypeList;
        public ObservableCollection<IdNamePair> EquipTypeList { get => equipTypeList; }

        /// <summary>
        /// 各装备类型所对应存储数据文件的文件名
        /// </summary>

        private readonly Dictionary<EquipType, string> equipDataFileNames = new Dictionary<EquipType, string>()
        {
            { EquipType.Body, "Armor_Body.json" },
            { EquipType.Arm, "Armor_Arm.json" },
            { EquipType.Waist, "Armor_Waist.json" },
            { EquipType.Leg, "Armor_Leg.json" },
            { EquipType.Head, "Armor_Head.json" },
            { EquipType.Talisman, "Talisman.json" },
            { EquipType.GreatSword, "Weapon_GreatSword.json" },
            { EquipType.SwordShield, "Weapon_SwordShield.json" },
            { EquipType.Hammer, "Weapon_Hammer.json" },
            { EquipType.Lance, "Weapon_Lance.json" },
            { EquipType.HeavyBowgun, "Weapon_HeavyBowgun.json" },
            { EquipType.LightBowgun, "Weapon_LightBowgun.json" },
            { EquipType.LongSword, "Weapon_LongSword.json" },
            { EquipType.SwitchAxe, "Weapon_SwitchAxe.json" },
            { EquipType.Gunlance, "Weapon_Gunlance.json" },
            { EquipType.Bow, "Weapon_Bow.json" },
            { EquipType.DualBlades, "Weapon_DualBlades.json" },
            { EquipType.HuntingHorn, "Weapon_HuntingHorn.json" }
        };

        /// <summary>
        /// 加载单个装备ID与名称文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="distList">接收的List</param>
        private void LoadEquipIdNameSingle(string filePath, out Dictionary<UInt32, string> dict, out ObservableCollection<IdNamePair> list)
        {
            dict = new Dictionary<UInt32, string>();
            list = new ObservableCollection<IdNamePair>();
            try
            {
                string jsonText = File.ReadAllText(filePath);
                var items = JsonConvert.DeserializeObject<List<IdNamePair>>(jsonText);
                if (items == null)
                {
                    MessageBox.Show($"文件{filePath}解析失败!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (var item in items)
                {
                    dict.Add(item.Id, item.Name);
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 读取所有装备ID与名称
        /// </summary>
        private void LoadEquipIdName()
        {
            /* 读取装备类型 */ 
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", "EquipType.json");

            if (File.Exists(filePath))
            {
                LoadEquipIdNameSingle(filePath, out EquipTypeDict, out equipTypeList);
            }
            else
            {
                MessageBox.Show($"找不到文件{filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /* 读取技能列表 */
            filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", "Skills.json");

            if (File.Exists(filePath))
            {
                LoadEquipIdNameSingle(filePath, out SkillDict, out skillList);
            }
            else
            {
                MessageBox.Show($"找不到文件{filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /* 读取道具列表 */
            filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", "Item.json");

            if (File.Exists(filePath))
            {
                LoadEquipIdNameSingle(filePath, out ItemDict, out itemList);
            }
            else
            {
                MessageBox.Show($"找不到文件{filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            /* 读取装饰珠列表 */
            filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", "Decorations.json");

            if (File.Exists(filePath))
            {
                LoadEquipIdNameSingle(filePath, out DecorationDict, out decorationList);
            }
            else
            {
                MessageBox.Show($"找不到文件{filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /* 读取全部装备 */
            for (EquipType i = EquipType.Body; i < EquipType.Count; i++)
            {
                if (equipDataFileNames.TryGetValue(i, out string fileName))
                {
                    filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
                    if (File.Exists(filePath))
                    {
                        LoadEquipIdNameSingle(filePath, out EquipDict[(int)i], out EquipList[(int)i]);
                    }
                    else
                    {
                        MessageBox.Show($"找不到文件{filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
