using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;

namespace SimpleOutline.Models
{
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

        public static OutlineItem CreateFromXmlElement(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            if (xmlElement.Name != "Item")
            {
                throw new DecodingException();
            }

            if (xmlElement.Attributes().SingleOrDefault(attr => attr.Name == "Name") == null)
            {
                throw new DecodingException();
            }

            if (xmlElement.Attributes().SingleOrDefault(attr => attr.Name != "Name") != null)
            {
                throw new DecodingException();
            }

            if (xmlElement.Elements().SingleOrDefault(elem => elem.Name != "Item") != null)
            {
                throw new DecodingException();
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
