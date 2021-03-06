using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Windows;

namespace SimpleOutline.Models
{
    [Serializable]
    public class OutlineItem : INotifyPropertyChanged
    {
        private string _name;
        private OutlineItemCollection _items;

        [NonSerialized]
        private OutlineItem _parent;

        [NonSerialized]
        private bool _isSelectedInView;
        
        [NonSerialized]
        private bool _isExpandedInView;

        [NonSerialized]
        private bool _isBeingEditedInView;

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

        public bool IsSelectedInView
        {
            get { return _isSelectedInView; }

            set
            {
                if (_isSelectedInView != value)
                {
                    _isSelectedInView = value;

                    OnPropertyChanged(nameof(IsSelectedInView));

                    if (!IsSelectedInView)
                    {
                        if (IsBeingEditedInView)
                            IsBeingEditedInView = false;
                    }
                }
            }
        }

        public bool IsExpandedInView
        {
            get { return _isExpandedInView; }

            set
            {
                if (_isExpandedInView != value)
                {
                    _isExpandedInView = value;

                    OnPropertyChanged(nameof(IsExpandedInView));

                    if (!IsExpandedInView)
                        IsExpandedInView = true;
                }
            }
        }

        public bool IsBeingEditedInView
        {
            get { return _isBeingEditedInView; }

            set
            {
                if (_isBeingEditedInView != value)
                {
                    _isBeingEditedInView = value;

                    OnPropertyChanged(nameof(IsBeingEditedInView));
                }
            }
        }


        public OutlineItem ParentItem
        {
            get { return _parent; }

            private set
            {
                _parent = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OutlineItem(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;

            Items = new OutlineItemCollection();

            Items.CollectionChanged += Items_CollectionChanged;
            IsExpandedInView = true;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (OutlineItem item in e.NewItems)
                {
                    if (item.ParentItem != null)
                    {
                        item.ParentItem.Items.Remove(item);
                    }

                    item.ParentItem = this;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (OutlineItem item in e.OldItems)
                {
                    item.ParentItem = null;
                }
            }
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

        public static OutlineItem LoadFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                var clipboardText = Clipboard.GetText();

                try
                {
                    var xmlElement = XElement.Parse(clipboardText);

                    var item = OutlineItem.CreateFromXmlElement(xmlElement);

                    return item;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
            /*
            var clipboardAdapter = new OutlineItemCollectionClipboardAdapter();
            var itemCollection = clipboardAdapter.GetItemCollection();

            if (itemCollection != null && itemCollection.Count > 0)
            {
                return itemCollection[0];
            }

            return null;*/
        }

        public void CopyToClipboard()
        {
            Clipboard.SetText(this.ToXmlElement().ToString());

            /*
            var itemCollection = new OutlineItemCollection();
            itemCollection.Add(this);

            var clipboardAdapter = new OutlineItemCollectionClipboardAdapter();
            clipboardAdapter.SetItemCollection(itemCollection);*/
        }
    }
}
