using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using SimpleOutline.Attributes;
using SimpleOutline.Data;

namespace SimpleOutline.Models
{
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
    public class OutlineItemCollection : ObservableCollection<OutlineItem>
    {
    }

    [UnusedCode, UnfinishedCode, UntestedCode]
    public class OutlineItem : INotifyPropertyChanged
    {
        private string _name;
        private OutlineItemCollection _items;

        public string Name
        {
            get { return _name; }
            
            set
            {
                if (_name == null)
                    throw new ArgumentNullException(nameof(Name));

                _name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        public OutlineItemCollection Items
        {
            get { return _items; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OutlineItem(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _name = name;

            _items = new OutlineItemCollection();
        }
        public void OnPropertyChanged(string propertyName)
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }

        public void CopyToClipboard()
        {
            var thisItemAsXmlElement = this.ToXmlElement();
            var thisItemAsXml = thisItemAsXmlElement.ToString();
            var clipboardData = new ClipboardData(thisItemAsXml);

            clipboardData.SaveToClipboard();
        }

        public static OutlineItem LoadFromClipboard()
        {
            var clipboardData = ClipboardData.LoadFromClipboard();
            var xmlElement = XElement.Parse(clipboardData.Data);
            var outlineItem = OutlineItem.CreateFromXmlElement(xmlElement);

            return outlineItem;
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
