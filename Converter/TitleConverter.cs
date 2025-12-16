using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MH3GHDSaveEditor.Converter
{
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            string Description = GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description;


            return $"{Description} {Version}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static T GetAssemblyAttribute<T>() where T : Attribute
        {
            var type = typeof(T);
            var attr = Assembly.GetExecutingAssembly().GetCustomAttribute(type);
            return attr as T;
        }
    }
}
