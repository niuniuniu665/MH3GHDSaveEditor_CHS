using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH3GHDSaveEditor.SaveDataModel
{
    public class AddressOffset
    {
        /// <summary>
        /// 角色名地址偏移量
        /// </summary>
        public Int32 CharacterNameOffset { get; set; }
        /// <summary>
        /// 角色名长度
        /// </summary>
        [JsonIgnore]
        public const int CharacterNameLength = 25;
        
        /// <summary>
        /// 角色性别地址偏移量
        /// </summary>
        public Int32 CharacterSexOffset { get; set; }
        
        /// <summary>
        /// 角色Zenny地址偏移量
        /// </summary>
        public Int32 CharacterZennyOffset { get; set; }
        
        /// <summary>
        /// 角色点数地址偏移量
        /// </summary>
        public Int32 CharacterPointOffset { get; set; }
        
        /// <summary>
        /// 角色道具地址偏移量
        /// </summary>
        public Int32 CharacterItemsOffset { get; set; }
        
        /// <summary>
        /// 角色装备地址偏移量
        /// </summary>
        public Int32 CharacterEquipOffset { get; set; }

        /// <summary>
        /// 存档大小（B）
        /// </summary>
        public Int32 SaveDataSize { get; set; }
    }
}
