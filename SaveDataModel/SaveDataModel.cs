using MH3GHDSaveEditor.SaveDataModel.Equip;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MH3GHDSaveEditor.SaveDataModel
{
    /// <summary>
    /// 存档数据模板，大端序
    /// </summary>
    public class SaveDataModel : INotifyPropertyChanged
    {

        private byte[] _saveData;
        /// <summary>
        /// 存档数据
        /// </summary>
        public byte[] SaveData
        {
            get => _saveData;
            set
            {
                _saveData = value;
                this.Initialize();
            }
        }

        private const int NameMaxinumLength = 24;
        private string _name = string.Empty;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name 
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    if (SaveData != null)
                    {
                        byte[] nameDst = new byte[NameMaxinumLength];
                        byte[] nameSrc = Encoding.UTF8.GetBytes(value);
                        Buffer.BlockCopy(nameSrc, 0, nameDst, 0, Math.Min(NameMaxinumLength, nameSrc.Length));
                        Buffer.BlockCopy(nameDst, 0, SaveData, AddressOffset.CharacterNameOffset, NameMaxinumLength);
                    }
                    OnPropertyChanged();
                }
            }
        }

        private int _sex = -1;
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex 
        {
            get => _sex;
            set
            {
                if (_sex != value)
                {
                    _sex = value;
                    if (SaveData != null)
                    {
                        SaveData[AddressOffset.CharacterSexOffset] = (byte)Sex;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Int32 _zenny;
        /// <summary>
        /// 金钱
        /// </summary>
        public Int32 Zenny 
        { 
            get => _zenny;
            set
            {
                if (_zenny != value)
                {
                    _zenny = value;
                    if (SaveData != null)
                    {
                        BinaryPrimitives.WriteInt32BigEndian(SaveData.AsSpan(AddressOffset.CharacterZennyOffset), value);
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Int32 _point;
        /// <summary>
        /// 资源点数
        /// </summary>
        public Int32 Point 
        { 
            get => _point;
            set
            {
                if (_point != value)
                {
                    _point = value;
                    if (SaveData != null)
                    {
                        BinaryPrimitives.WriteInt32BigEndian(SaveData.AsSpan(AddressOffset.CharacterPointOffset), value);
                    }
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 道具箱容量
        /// </summary>
        private const int ItemBoxMaxinum = 1000;

        /// <summary>
        /// 道具
        /// </summary>
        public Item[] items = new Item[ItemBoxMaxinum];

        /// <summary>
        /// 装备
        /// </summary>
        public EquipData[] equips = new EquipData[ItemBoxMaxinum];

        /// <summary>
        /// 数据地址偏移量
        /// </summary>
        public AddressOffset AddressOffset { get; set; }

        public void Initialize()
        {
            if (SaveData == null)
            {
                Name = String.Empty;
                Sex = -1;
                Zenny = 0;
                Point = 0;

                for (int i = 0; i < ItemBoxMaxinum; i++)
                {
                    items[i] = new Item();
                    equips[i] = new EquipData();
                }
            }
            else
            {
                Name = Encoding.UTF8.GetString(SaveData, AddressOffset.CharacterNameOffset, AddressOffset.CharacterNameLength);
                Sex = SaveData[AddressOffset.CharacterSexOffset];
                Zenny = BinaryPrimitives.ReadInt32BigEndian(SaveData.AsSpan(AddressOffset.CharacterZennyOffset, 4));
                Point = BinaryPrimitives.ReadInt32BigEndian(SaveData.AsSpan(AddressOffset.CharacterPointOffset, 4));

                for (int i = 0; i < ItemBoxMaxinum; i++)
                {
                    // Item
                    items[i] = new Item
                    {
                        ListRow = i + 1
                    };
                    items[i].SetItemData(SaveData.AsSpan(AddressOffset.CharacterItemsOffset + i * Item.ItemSize, Item.ItemSize).ToArray());
                    

                    // Equip
                    equips[i] = new EquipData
                    {
                        ListRow = i + 1
                    };
                    equips[i].SetEquipData(SaveData.AsSpan(AddressOffset.CharacterEquipOffset + i * EquipData.EquipDataSize, EquipData.EquipDataSize).ToArray());
                    
                }
            }
        }

        public SaveDataModel()
        {
            AddressOffset = new AddressOffset();
        }

        public void Save2DataBytes()
        {
            for (int i = 0; i < ItemBoxMaxinum; i++)
            {
                Buffer.BlockCopy(items[i].GetItemData(), 0, _saveData, 
                    AddressOffset.CharacterItemsOffset + i * Item.ItemSize, Item.ItemSize);
                Buffer.BlockCopy(equips[i].GetEquipData(), 0, _saveData, 
                    AddressOffset.CharacterEquipOffset + i * EquipData.EquipDataSize, EquipData.EquipDataSize);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
