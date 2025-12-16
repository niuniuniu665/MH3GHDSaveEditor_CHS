using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MH3GHDSaveEditor.Converter
{
    /// <summary>
    /// 将整型变量进行转换
    /// </summary>
    public class IntValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ChangeType(value, targetType);
            }
            catch 
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ChangeType(value, targetType);
            }
            catch
            {
                return 0;
            }
        }
    }
}
