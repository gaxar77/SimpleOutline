using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace SimpleOutline.Models
{
    public class OutlineDocument : INotifyPropertyChanged
    {
        private string _fileName;
        private OutlineItemCollection _items;

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

            if (xmlOutlineElement.Elements().Any(e => e.Name != "Item"))
            {
                throw new DecodingException();
            }

            var thisOutlineDocument = new OutlineDocument();

            foreach (var itemXmlElement in xmlOutlineElement.Elements("Item"))
            {
                var outlineItem = OutlineItem.CreateFromXmlElement(itemXmlElement);
                thisOutlineDocument.Items.Add(outlineItem);
            }

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

            return thisOutlineDocument;
        }
    }
}