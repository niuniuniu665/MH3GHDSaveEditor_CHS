using MH3GHDSaveEditor.SaveDataModel.Equip;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MH3GHDSaveEditor.SaveDataModel
{
    /// <summary>
    /// 道具，大端序
    /// </summary>
    public class Item : INotifyPropertyChanged
    {
        public int ListRow { get; set; }

        /// <summary>
        /// 道具数据的大小
        /// </summary>
        public const int ItemSize = 4;

        private UInt16 _id;
        /// <summary>
        /// 道具ID
        /// </summary>
        public UInt16 Id 
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    if (MainWindow.Instance.ItemDict.TryGetValue(value, out string name))
                    {
                        this.IdName.Name = name;
                    }
                    else
                    {
                        this.IdName.Name = "未知道具";
                    }
                    OnPropertyChanged();
                }
            }
        }
        private const int IdOffset = 0;

        private IdNamePair _idName = new IdNamePair();
        public IdNamePair IdName
        { 
            get => _idName;
            set
            {
                if (_idName.Id != value.Id)
                {
                    Id = (ushort)value.Id;
                    _idName.Id = value.Id;
                    _idName.Name = value.Name;
                    OnPropertyChanged();
                }
            }
        }

        private UInt16 _count;
        /// <summary>
        /// 道具数量
        /// </summary>
        public UInt16 Count 
        {
            get => _count;
            set 
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged();
                }
            }
        }
        private const int CountOffset = 2;

        public void SetItemData(byte[] data)
        {
            if (data == null || data.Length != ItemSize)
            {
                return;
            }
            Id = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(IdOffset, 2));
            Count = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(CountOffset, 2));
            IdName.Id = Id;
            if (MainWindow.Instance.ItemDict.TryGetValue(Id, out string name))
            {
                IdName.Name = name;
            }
            else
            {
                IdName.Name = "未知道具";
            }
        }

        public void SetItemData(byte[] data, int start)
        {
            if (data == null || data.Length - start < ItemSize)
            {
                return;
            }
            SetItemData(data.AsSpan(start, ItemSize).ToArray());
        }

        public byte[] GetItemData()
        {
            byte[] data = new byte[ItemSize];

            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(IdOffset), Id);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(CountOffset), Count);

            return data;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
