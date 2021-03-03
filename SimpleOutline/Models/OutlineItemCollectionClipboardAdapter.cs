using System.Windows;

namespace SimpleOutline.Models
{
    public class OutlineItemCollectionClipboardAdapter
    {
        private DataFormat _format;
        public DataFormat Format
        {
            get
            {
                if (_format == null)
                {
                    _format = DataFormats.GetDataFormat(typeof(OutlineItemCollection).FullName);
                }

                return _format;
            }
        }

        public void SetItemCollection(OutlineItemCollection item)
        {
            var dataObject = new DataObject(Format.Name, item);
            Clipboard.SetDataObject(dataObject, true);
        }

        public OutlineItemCollection GetItemCollection()
        {
            if (Clipboard.ContainsData(Format.Name))
            {
                var dataObject = Clipboard.GetDataObject();

                return (OutlineItemCollection)dataObject.GetData(Format.Name);
            }

            return null;
        }
    }
}
