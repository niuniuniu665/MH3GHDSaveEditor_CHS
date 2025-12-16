using MH3GHDSaveEditor.SaveDataModel.Equip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MH3GHDSaveEditor.Converter
{
    public class EquipTypeUIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(uint))
            {
                return (uint)(value ?? 0);
            }
            else if (targetType == typeof(EquipType))
            {
                return (EquipType)(uint)(value ?? 0);
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(uint))
            {
                return (uint)(value ?? 0);
            }
            else if (targetType == typeof(EquipType))
            {
                return (EquipType)(uint)(value ?? 0);
            }
            else
            {
                return value;
            }
        }
    }
}
