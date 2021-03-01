using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SimpleOutline.Data
{
    public class ClipboardDataUnexpectedFormatException : Exception
    {
        private const string DefaultMessage = "The data could not be loaded from the clipboard because a different data from was expected. ";
        public ClipboardDataUnexpectedFormatException()
            : base(DefaultMessage)
        {

        }
    }
    public class ClipboardData
    {
        private string _data;

        public string Data
        {
            get
            {
                return _data;
            }
            
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Data));

                _data = value;
            }
        }
        public ClipboardData(string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            Data = data;
        }

        public void SaveToClipboard()
        {
            var format = DataFormats.GetFormat(typeof(ClipboardData).FullName);

            var dataObject = new DataObject(format.Name, Data);

            Clipboard.SetDataObject(dataObject);
        }

        public static ClipboardData LoadFromClipboard()
        {
            IDataObject dataObject = Clipboard.GetDataObject();

            if (!dataObject.GetFormats().Any(format => format == typeof(ClipboardData).FullName))
            {
                throw new ClipboardDataUnexpectedFormatException();
            }

            var data = (string)dataObject.GetData(typeof(ClipboardData).FullName);
            var clipboardData = new ClipboardData(data);

            return clipboardData;
        }
    }
}
