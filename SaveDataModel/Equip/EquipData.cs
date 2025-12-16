using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MH3GHDSaveEditor.SaveDataModel.Equip
{
    /// <summary>
    /// 装备数据，大端序
    /// </summary>
    public class EquipData : INotifyPropertyChanged
    {
        public int ListRow { get; set; }

        /// <summary>
        /// 装备数据的大小
        /// </summary>
        public const int EquipDataSize = 16;

        private EquipType _equipType;
        /// <summary>
        /// 装备类型
        /// </summary>
        public EquipType EquipType 
        {
            get => _equipType;
            set
            {
                if (_equipType != value)
                {
                    _equipType = value;
                    EquipDataOptionsIsEnableChange();
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// 装备类型 数据位置偏移量
        /// </summary>
        private const int EquipTypeOffSet = 0;

        private IdNamePair _equipTypeIdName = new IdNamePair();
        public IdNamePair EquipTypeIdName
        {
            get => _equipTypeIdName;
            set
            {
                if (_equipTypeIdName.Id != value.Id)
                {
                    EquipType = (EquipType)value.Id;
                    _equipTypeIdName.Id = value.Id;
                    _equipTypeIdName.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 防具强化等级或护石孔数
        /// </summary>
        public byte LevelorSlots { get; set; }

        private bool _levelorSlotsIsEnabled;
        public bool LevelorSlotsIsEnabled 
        { 
            get => _levelorSlotsIsEnabled;
            set
            {
                if (_levelorSlotsIsEnabled != value)
                {
                    _levelorSlotsIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 防具强化等级或护石孔数 数据位置偏移量
        /// </summary>
        private const int LevelorSlotsOffSet = 1;

        private UInt16 _equipID;
        /// <summary>
        /// 装备ID
        /// </summary>
        public UInt16 EquipID 
        {
            get => _equipID;
            set
            {
                if (_equipID != value)
                {
                    _equipID = value;
                    if (EquipType != EquipType.Null)
                    {
                        MainWindow.Instance.EquipDict[(uint)EquipType].TryGetValue(EquipID, out string equipName);
                        EquipIdName = new IdNamePair() { Id = EquipID, Name = string.IsNullOrEmpty(equipName) ? "未知装备" : equipName };
                    }
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 装备ID 数据位置偏移量
        /// </summary>
        private const int EquipIDOffSet = 2;

        private IdNamePair _equipIdName = new IdNamePair();
        public IdNamePair EquipIdName
        {
            get => _equipIdName;
            set
            {
                if (_equipIdName.Id != value.Id)
                {
                    EquipID = (UInt16)value.Id;
                    _equipIdName.Id = value.Id;
                    _equipIdName.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 装备名称列表
        /// </summary>
        public ObservableCollection<IdNamePair> EquipNameList
        {
            get
            {
                return MainWindow.Instance.EquipList[(int)EquipType];
            }
        }

        private bool _skillIsEnabled;
        /// <summary>
        /// 护石技能是否启用
        /// </summary>
        public bool SkillIsEnabled
        {
            get => _skillIsEnabled;
            set
            {
                if (_skillIsEnabled != value)
                {
                    _skillIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        #region 护石技能1
        private byte _skill_1;
        /// <summary>
        /// 护石技能1
        /// </summary>
        public byte Skill_1 
        {
            get => _skill_1;
            set
            {
                if (_skill_1 != value)
                {
                    _skill_1 = value;
                    if (MainWindow.Instance.SkillDict.TryGetValue(Skill_1, out string skillName))
                    {
                        Skill_1Name = new IdNamePair() { Id = Skill_1, Name = string.IsNullOrEmpty(skillName) ? "未知技能" : skillName };
                    }
                    else
                    {
                        Skill_1Name = new IdNamePair() { Id = 0, Name = "无技能"};
                    }
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 护石技能1 数据位置偏移量
        /// </summary>
        private const int Skill_1OffSet = 4;
        private IdNamePair _skill_1Name = new IdNamePair();
        public IdNamePair Skill_1Name
        {
            get => _skill_1Name;
            set
            {
                if (_skill_1Name.Id != value.Id)
                {
                    Skill_1 = (byte)value.Id;
                    _skill_1Name.Id = value.Id;
                    _skill_1Name.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 护石技能1点数
        /// </summary>
        public byte Skill_1Point { get; set; }
        /// <summary>
        /// 护石技能1点数 数据位置偏移量
        /// </summary>
        private const int Skill_1PointOffSet = 5;
        #endregion

        #region 护石技能2
        private byte _skill_2;
        /// <summary>
        /// 护石技能2
        /// </summary>
        public byte Skill_2
        {
            get => _skill_2;
            set
            {
                if (_skill_2 != value)
                {
                    _skill_2 = value;
                    if (MainWindow.Instance.SkillDict.TryGetValue(Skill_2, out string skillName))
                    {
                        Skill_2Name = new IdNamePair() { Id = Skill_2, Name = string.IsNullOrEmpty(skillName) ? "未知技能" : skillName };
                    }
                    else
                    {
                        Skill_2Name = new IdNamePair() { Id = 0, Name = "无技能" };
                    }
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 护石技能2 数据位置偏移量
        /// </summary>
        private const int Skill_2OffSet = 6;

        private IdNamePair _skill_2Name = new IdNamePair();
        public IdNamePair Skill_2Name
        {
            get => _skill_2Name;
            set
            {
                if (_skill_2Name.Id != value.Id)
                {
                    Skill_2 = (byte)value.Id;
                    _skill_2Name.Id = value.Id;
                    _skill_2Name.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 护石技能2点数
        /// </summary>
        public byte Skill_2Point { get; set; }
        /// <summary>
        /// 护石技能2点数 数据位置偏移量
        /// </summary>
        private const int Skill_2PointOffSet = 7;
        #endregion

        #region 装饰珠1
        private UInt16 _decoration_1;
        /// <summary>
        /// 装饰珠1
        /// </summary>
        public UInt16 Decoration_1 
        { 
            get => _decoration_1;
            set
            {
                if (value != _decoration_1)
                {
                    _decoration_1 = value;
                    // todo: 装饰珠字典
                    OnPropertyChanged();
                }
            } 
        }

        private IdNamePair _decoration_1Name = new IdNamePair();
        public IdNamePair Decoration_1Name
        {
            get => _decoration_1Name;
            set
            {
                if (_decoration_1Name.Id != value.Id)
                {
                    Decoration_1 = (UInt16)value.Id;
                    _decoration_1Name.Id = value.Id;
                    _decoration_1Name.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 装饰珠1 数据位置偏移量
        /// </summary>
        private const int Decoration_1OffSet = 8;
        #endregion

        #region 装饰珠2
        private UInt16 _decoration_2;
        /// <summary>
        /// 装饰珠2
        /// </summary>
        public UInt16 Decoration_2
        {
            get => _decoration_2;
            set
            {
                if (value != _decoration_2)
                {
                    _decoration_2 = value;
                    // todo: 装饰珠字典
                    OnPropertyChanged();
                }
            }
        }

        private IdNamePair _decoration_2Name = new IdNamePair();
        public IdNamePair decoration_2Name
        {
            get => _decoration_2Name;
            set
            {
                if (_decoration_2Name.Id != value.Id)
                {
                    Decoration_2 = (UInt16)value.Id;
                    _decoration_2Name.Id = value.Id;
                    _decoration_2Name.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 装饰珠2 数据位置偏移量
        /// </summary>
        private const int Decoration_2OffSet = 10;
        #endregion

        #region 装饰珠3
        private UInt16 _decoration_3;
        /// <summary>
        /// 装饰珠3
        /// </summary>
        public UInt16 Decoration_3
        {
            get => _decoration_3;
            set
            {
                if (value != _decoration_3)
                {
                    _decoration_3 = value;
                    // todo: 装饰珠字典
                    OnPropertyChanged();
                }
            }
        }

        private IdNamePair _decoration_3Name = new IdNamePair();
        public IdNamePair Decoration_3Name
        {
            get => _decoration_3Name;
            set
            {
                if (_decoration_3Name.Id != value.Id)
                {
                    Decoration_3 = (UInt16)value.Id;
                    _decoration_3Name.Id = value.Id;
                    _decoration_3Name.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 装饰珠3 数据位置偏移量
        /// </summary>
        private const int Decoration_3OffSet = 12;
        #endregion

        public void SetEquipData(byte[] data)
        {
            if (data == null || data.Length != EquipDataSize)
            {
                return;
            }
            EquipType = (EquipType)data[EquipTypeOffSet];
            LevelorSlots = data[LevelorSlotsOffSet];
            EquipID = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(EquipIDOffSet, 2));
            
            if (EquipType == EquipType.Talisman)
            {
                Skill_1 = data[Skill_1OffSet];
                Skill_1Point = data[Skill_1PointOffSet];
                Skill_2 = data[Skill_2OffSet];
                Skill_2Point = data[Skill_2PointOffSet];
            }
            Decoration_1 = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(Decoration_1OffSet, 2));
            Decoration_2 = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(Decoration_2OffSet, 2));
            Decoration_3 = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(Decoration_3OffSet, 2));

            EquipTypeIdName.Id = (uint)EquipType;
            if (MainWindow.Instance.EquipTypeDict.TryGetValue((uint)EquipType, out string name))
            {
                EquipTypeIdName.Name = name;
            }
            else
            {
                EquipTypeIdName.Name = "未知类型";
            }
            
            EquipIdName.Id = EquipID;
            if (EquipType != EquipType.Null && MainWindow.Instance.EquipDict[(uint)EquipType].TryGetValue(EquipID, out name))
            {
                EquipIdName.Name = name;
            }
            else if (EquipType == EquipType.Null)
            {
                EquipIdName.Name = "无装备";
            }
            else
            {
                EquipIdName.Name = "未知装备";
            }
        }

        public void SetEquipData(byte[] data, int start)
        {
            if (data == null || data.Length - start < EquipDataSize)
            {
                return;
            }
            SetEquipData(data.AsSpan(start, EquipDataSize).ToArray());
        }

        public byte[] GetEquipData()
        {
            byte[] data = new byte[EquipDataSize];
            if (EquipType == EquipType.Null || EquipType == EquipType.Count)
            {
                return data;
            }

            data[EquipTypeOffSet] = (byte)EquipType;
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(EquipIDOffSet), EquipID);

            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(Decoration_1OffSet), Decoration_1);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(Decoration_2OffSet), Decoration_2);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(Decoration_3OffSet), Decoration_3);

            switch (EquipType)
            {
                case EquipType.Body:
                case EquipType.Arm:
                case EquipType.Waist:
                case EquipType.Leg:
                case EquipType.Head:
                    data[LevelorSlotsOffSet] = LevelorSlots;
                    break;
                case EquipType.Talisman:
                    data[LevelorSlotsOffSet] = LevelorSlots;
                    data[Skill_1OffSet] = Skill_1;
                    data[Skill_1PointOffSet] = Skill_1Point;
                    data[Skill_2OffSet] = Skill_2;
                    data[Skill_2PointOffSet] = Skill_2Point;
                    break;
            }
            return data;
        }

        /// <summary>
        /// 改变装备所有选项的可用性
        /// </summary>
        private void EquipDataOptionsIsEnableChange()
        {
            LevelorSlotsIsEnabled = (EquipType >= EquipType.Body && EquipType <= EquipType.Talisman);
            SkillIsEnabled = (EquipType == EquipType.Talisman);
            EquipTypeIdName = new IdNamePair() { Id = (uint)EquipType, Name = MainWindow.Instance.EquipTypeDict[(uint)EquipType] };
            EquipIdName = new IdNamePair() { Id = 0, Name = "无装备" };

            if (EquipType == EquipType.Talisman)
            {
                Skill_2 = Skill_1 = byte.MaxValue;
                Skill_2Name = Skill_1Name = new IdNamePair() { Id = 0, Name = "无技能" };
                Skill_2Point = Skill_1Point = 0;
            }
            else
            {
                Skill_2Name = Skill_1Name = new IdNamePair() { Id = 0, Name = string.Empty };
                Skill_2Point = Skill_1Point = 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
