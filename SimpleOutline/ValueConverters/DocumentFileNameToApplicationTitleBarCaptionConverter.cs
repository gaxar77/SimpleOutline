using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SimpleOutline.Models;

namespace SimpleOutline.ValueConverters
{
    public class IsItemBeingEditedUIElementVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBeingEditedInView = (bool)value;

            if ((string)parameter == "NameDisplayText")
            {
                return isBeingEditedInView ? Visibility.Collapsed : Visibility.Visible;
            }
            else if ((string)parameter == "NameEditField")
            {
                return isBeingEditedInView ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
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
