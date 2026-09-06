using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH3GHDSaveEditor.SaveDataModel.Equip
{
    /// <summary>
    /// 装备套装数据，大端序 装备ID为道具箱中的顺序
    /// </summary>
    public class EquipSet
    {
        /// <summary>
        /// 套装个数
        /// </summary>
        public const int EquipSetCount = 24;

        /// <summary>
        /// 装备套装数据块大小
        /// </summary>
        public const int EquipSetDataSize = 76;

        /// <summary>
        /// 武器ID的偏移量
        /// </summary>
        private const int weaponIdOffset = 0;

        /// <summary>
        /// 武器ID
        /// </summary>
        public UInt16 WeaponId { get; set; }

        /// <summary>
        /// 武器种类
        /// </summary>
        public EquipType WeaponType 
        {
            get
            {
                if (WeaponId >= 0 && WeaponId < SaveDataModel.ItemBoxMaxinum)
                {
                    return MainWindow.Instance.saveDataModel.equips[WeaponId].EquipType;
                }
                return EquipType.Null;
            }
        }

        /// <summary>
        /// 胴装备ID的偏移量
        /// </summary>
        private const int bodyIdOffset = 2;

        /// <summary>
        /// 胴装备ID
        /// </summary>
        public UInt16 BodyId { get; set; }

        /// <summary>
        /// 手装备ID的偏移量
        /// </summary>
        private const int armIdOffset = 4;

        /// <summary>
        /// 手装备ID
        /// </summary>
        public UInt16 ArmId { get; set; }

        /// <summary>
        /// 腰装备ID的偏移量
        /// </summary>
        private const int waistIdOffset = 6;

        /// <summary>
        /// 腰装备ID
        /// </summary>
        public UInt16 WaistId { get; set; }

        /// <summary>
        /// 腿装备ID的偏移量
        /// </summary>
        private const int legIdOffset = 8;

        /// <summary>
        /// 腿装备ID
        /// </summary>
        public UInt16 LegId { get; set; }

        /// <summary>
        /// 头装备ID的偏移量
        /// </summary>
        private const int headIdOffset = 10;

        /// <summary>
        /// 头装备ID
        /// </summary>
        public UInt16 HeadId { get; set; }

        /// <summary>
        /// 护石ID的偏移量
        /// </summary>
        private const int talismanIdOffset = 12;

        /// <summary>
        /// 护石ID 0xFFFF为空
        /// </summary>
        public UInt16 TalismanId { get; set; }

        /// <summary>
        /// 装饰珠ID个数
        /// </summary>
        private const int DecorationCount = 21;

        /// <summary>
        /// 装饰珠ID偏移量
        /// </summary>
        private const int decorationOffset = 14;

        /// <summary>
        /// 装饰珠列表 (每三个Slot一组，按顺序分别为武器、头、胴、手、腰、腿、护石的装饰珠)
        /// </summary>
        public UInt16[] DecorationList = new UInt16[DecorationCount];

        /// <summary>
        /// 套装名的偏移量
        /// </summary>
        private const int equipSetNameOffset = 56;

        /// <summary>
        /// 套装名最长长度
        /// </summary>
        private const int equipSetNameMaxLength = 20;

        /// <summary>
        /// 套装名
        /// </summary>
        public string EquipSetName { get; set; }

        /// <summary>
        /// 设置装备套装数据
        /// </summary>
        /// <param name="data">数据</param>
        public void SetEquipSetData(byte[] data)
        {
            if (data == null || data.Length != EquipSetDataSize)
            {
                return;
            }

            WeaponId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(weaponIdOffset, sizeof(UInt16)));
            BodyId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(bodyIdOffset, sizeof(UInt16)));
            ArmId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(armIdOffset, sizeof(UInt16)));
            WaistId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(waistIdOffset, sizeof(UInt16)));
            LegId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(legIdOffset, sizeof(UInt16)));
            HeadId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(headIdOffset, sizeof(UInt16)));
            TalismanId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(talismanIdOffset, sizeof(UInt16)));

            for (int i = 0; i < DecorationCount; i ++)
            {
                DecorationList[i] = BinaryPrimitives.ReadUInt16BigEndian(
                    data.AsSpan(decorationOffset + sizeof(UInt16) * i, 
                    sizeof(UInt16)));
            }

            EquipSetName = Encoding.UTF8.GetString(data, equipSetNameOffset, equipSetNameMaxLength);
        }

        /// <summary>
        /// 设置装备套装数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="start">读取数据的起始位置</param>
        public void SetEquipSetData(byte[] data, int start)
        {
            if (data == null || data.Length - start < EquipSetDataSize)
            {
                return;
            }
            SetEquipSetData(data.AsSpan(start, EquipSetDataSize).ToArray());
        }

        /// <summary>
        /// 获取装备套装数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetEquipSetData()
        {
            byte[] data = new byte[EquipSetDataSize];

            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(weaponIdOffset), WeaponId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(bodyIdOffset), BodyId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(armIdOffset), ArmId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(waistIdOffset), WaistId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(legIdOffset), LegId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(headIdOffset), HeadId);
            BinaryPrimitives.WriteUInt16BigEndian(data.AsSpan(talismanIdOffset), TalismanId);

            for (int i = 0; i < DecorationCount; i++)
            {
                BinaryPrimitives.WriteUInt16BigEndian(
                    data.AsSpan(decorationOffset + sizeof(UInt16) * i),
                    DecorationList[i]);
            }

            Array.Clear(data, equipSetNameOffset, equipSetNameMaxLength);
            byte[] nameSrc = Encoding.UTF8.GetBytes(EquipSetName);
            Buffer.BlockCopy(nameSrc, 0, data, equipSetNameOffset, Math.Min(equipSetNameMaxLength, nameSrc.Length));
            data[equipSetNameOffset + equipSetNameMaxLength - 1] = byte.MinValue;

            return data;
        }
    }
}
