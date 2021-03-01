using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows;
using SimpleOutline.Attributes;

namespace SimpleOutline.Models
{
    [UnfinishedCode]
    public class ViewModel
    {
        public ObservableCollection<OutlineDocument> Documents { get; private set; }


    }
    [UntestedCode]
    public class OutlineDocument : INotifyPropertyChanged
    {
        private string _fileName;
        private OutlineItem _rootItem;

        public string FileName
        {
            get { return _fileName; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(FileName));

                _fileName = value;

                OnPropertyChanged(nameof(FileName));
            }
        }

        public OutlineItem RootItem
        {
            get { return _rootItem; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }

        public OutlineDocument(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            FileName = fileName;

            _rootItem = new OutlineItem("Root Item");
        }

        public OutlineDocument()
            : this("Untitled.sof")
        {

        }
    }
    public class OutlineDecodingException : Exception
    {
        private const string DefaultMessage = "An error occured while trying to decode outline data.";
        public OutlineDecodingException()
            : base(DefaultMessage)
        {

        }

        public OutlineDecodingException(Exception innerException)
            : base(DefaultMessage, innerException)
        {

        }
    }

    [Todo("Replace with custom collection class that rejects null and duplicate items.")]
    [Serializable]
    public class OutlineItemCollection : ObservableCollection<OutlineItem>
    {
    }

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
    [UnusedCode, UnfinishedCode, UntestedCode]
    [Serializable]
    public class OutlineItem : INotifyPropertyChanged
    {
        private string _name;
        private OutlineItemCollection _items;

        public string Name
        {
            get { return _name; }
            
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Name));

                _name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        public OutlineItemCollection Items
        {
            get { return _items; }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Items));
                }

                _items = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OutlineItem(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;

            Items = new OutlineItemCollection();
        }
        public void OnPropertyChanged(string propertyName)
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
        public XElement ToXmlElement()
        {
            var thisItemXmlElement = new XElement("Item", new XAttribute("Name", Name));

            foreach (OutlineItem childItem in Items)
            {
                var childItemXmlElement = childItem.ToXmlElement();

                thisItemXmlElement.Add(childItemXmlElement);
            }

            return thisItemXmlElement;
        }

        [Todo("Refactor into decoder/deserializer class (whatever you want to call it).")]
        public static OutlineItem CreateFromXmlElement(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            if (xmlElement.Name != "Item")
            {
                throw new OutlineDecodingException();
            }

            if (xmlElement.Attributes().SingleOrDefault(attr => attr.Name == "Name") == null)
            {
                throw new OutlineDecodingException();
            }

            if (xmlElement.Attributes().SingleOrDefault(attr => attr.Name != "Name") != null)
            {
                throw new OutlineDecodingException();
            }

            if (xmlElement.Elements().SingleOrDefault(elem => elem.Name != "Item") != null)
            {
                throw new OutlineDecodingException();
            }

            var thisOutlineItemName = xmlElement.Attribute("Name").Value.ToString();
            var thisOutlineItem = new OutlineItem(thisOutlineItemName);
            foreach (XElement childOutlineItemXmlElement in xmlElement.Elements("Item"))
            {
                var childOutlineItem = OutlineItem.CreateFromXmlElement(childOutlineItemXmlElement);
                thisOutlineItem.Items.Add(childOutlineItem);
            }

            return thisOutlineItem;
        }
    }
}
