using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH3GHDSaveEditor.SaveDataModel.Equip
{
    /// <summary>
    /// 装备类型
    /// </summary>
    public enum EquipType
    {
        Null = 0,               // 无
        Body = 0x01,            // 胴
        Arm = 0x02,             // 手
        Waist = 0x03,           // 腰
        Leg = 0x04,             // 腿
        Head = 0x05,            // 头
        Talisman = 0x06,        // 护石
        GreatSword = 0x07,      // 大剑
        SwordShield = 0x08,     // 片手剑
        Hammer = 0x09,          // 锤
        Lance = 0x0A,           // 长枪
        HeavyBowgun = 0x0B,     // 重弩
        LightBowgun = 0x0D,     // 轻弩
        LongSword = 0x0E,       // 太刀
        SwitchAxe = 0x0F,       // 斩斧
        Gunlance = 0x10,        // 铳枪
        Bow = 0x11,             // 弓
        DualBlades = 0x12,      // 双剑
        HuntingHorn = 0x13,     // 狩猎笛

        Count   // EquipType的枚举个数
    }
}
