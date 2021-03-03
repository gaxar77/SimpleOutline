using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SimpleOutline.ValueConverters
{
    public class DocumentFileNameToApplicationTitleBarCaptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fileName = (string)value;
            var titleBarCaption = $"SimpleOutline 1.0 - {fileName}";

            return titleBarCaption;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
