using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH3GHDSaveEditor.SaveDataModel
{
    /// <summary>
    /// ID与名称对，用于读取Json中的数据
    /// </summary>
    public class IdNamePair
    {
        public UInt32 Id { get; set; }
        public string Name { get; set; }
    }
}
