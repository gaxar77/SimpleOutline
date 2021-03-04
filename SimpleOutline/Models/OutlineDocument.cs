using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace SimpleOutline.Models
{
    public enum OutlineDocumentState
    {
        NewDocument,
        LoadedDocument,
        SavedDocument
    }
    public class OutlineDocument : INotifyPropertyChanged
    {
        private string _fileName;
        private OutlineItemCollection _items;
        private OutlineDocumentState _state;

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

        public OutlineDocumentState State
        {
            get { return _state; }

            set
            {
                _state = value;

                OnPropertyChanged(nameof(State));
            }
        }

        public OutlineItemCollection Items
        {
            get { return _items; }
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

            _items = new OutlineItemCollection();
            _items.Add(new OutlineItem("Outline"));

            State = OutlineDocumentState.NewDocument;
        }

        public OutlineDocument()
            : this("Untitled.sof")
        {
        
        }

        public XElement ToXmlElement()
        {
            var documentElement = new XElement("SimpleOutlineDocument");
            var outlineElement = new XElement("Outline");

            foreach (var item in Items)
            {
                var itemElement = item.ToXmlElement();
                outlineElement.Add(itemElement);
            }

            documentElement.Add(outlineElement);

            return documentElement;
        }

        public XDocument ToXmlDocument()
        {
            var documentElement = ToXmlElement();
            var xmlDocument = new XDocument(documentElement);

            return xmlDocument;
        }

        public static OutlineDocument CreateFromXmlElement(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            if (xmlElement.Name != "SimpleOutlineDocument")
            {
                throw new DecodingException();
            }

            if (xmlElement.Elements().SingleOrDefault(e => e.Name != "Outline") != null)
            {
                throw new DecodingException();
            }

            if (xmlElement.Elements().SingleOrDefault(e => e.Name == "Outline") == null)
            {
                throw new DecodingException();
            }

            var xmlOutlineElement = xmlElement.Element("Outline");

            if (xmlOutlineElement.Elements("Item").Count() != 1)
            {
                throw new DecodingException();
            }

            var thisOutlineDocument = new OutlineDocument();

            var rootItemXmlElement = xmlOutlineElement.Element("Item");
            var rootOutlineItem = OutlineItem.CreateFromXmlElement(rootItemXmlElement);
            thisOutlineDocument.Items[0] = rootOutlineItem;

            return thisOutlineDocument;
        }

        public static OutlineDocument CreateFromXmlDocument(XDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw new ArgumentNullException(nameof(xmlDocument));

            if (xmlDocument.Root.Name != "SimpleOutlineDocument")
                throw new DecodingException();

            var thisDocument = CreateFromXmlElement(xmlDocument.Root);

            return thisDocument;
        }

        public static OutlineDocument Load(string fileName)
        {
            var xmlDocument = XDocument.Load(fileName);
            var thisOutlineDocument = OutlineDocument.CreateFromXmlDocument(xmlDocument);

            thisOutlineDocument.FileName = fileName;
            thisOutlineDocument.State = OutlineDocumentState.LoadedDocument;

            return thisOutlineDocument;
        }

        public void Save()
        {
            Save(FileName);
        }

        public void Save(string fileName)
        {
            var xmlDocument = ToXmlDocument();
            xmlDocument.Save(fileName);

            FileName = fileName;
            State = OutlineDocumentState.SavedDocument;
        }
    }
}