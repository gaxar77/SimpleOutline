using System.Collections.Generic;
using System.Xml.Linq;

namespace SimpleOutline
{
    public class OutlineItem
    {
        public string Name { get; set; }

        public List<OutlineItem> Items { get; set; } =
            new List<OutlineItem>();

        public OutlineItem ParentItem { get; set; }

        public OutlineItem(string name)
        {
            this.Name = name;
        }
        public OutlineItem(OutlineItem item)
        {
            this.Name = item.Name;

            this.ParentItem = item.ParentItem;

            foreach (OutlineItem childItem in item.Items)
            {
                var newChildItem = new OutlineItem(childItem);
                this.Items.Add(newChildItem);
            }
        }

        public OutlineItem(XElement itemElement)
        {
            if (itemElement.Name != "SimpleOutlineItem")
            {
                throw new System.Exception();
            }

            this.Name = itemElement.Attribute("Name").Value.ToString();

            foreach (XElement childItemElement in itemElement.Elements("SimpleOutlineItem"))
            {
                var childItem = new OutlineItem(childItemElement);
    
                childItem.ParentItem = this;

                this.Items.Add(childItem);
            }
        }

        public XElement ToXElement()
        {
            var itemElement = new XElement("SimpleOutlineItem", new XAttribute("Name", this.Name));
            
            foreach (OutlineItem childItem in this.Items)
            {
                var childItemElement = childItem.ToXElement();
                itemElement.Add(childItemElement);
            }

            return itemElement;
        }
    }
}
