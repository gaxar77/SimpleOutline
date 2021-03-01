using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using System.Windows.Forms;
using SimpleOutline.Models;

namespace UnitTests
{
    [TestClass]
    public class OutlineItemClipboardMethodTest
    {
        [TestMethod]
        public void OutlineItemsStoredInAndRetrievedFromClipboardEqual()
        {
            var outlineItem1 = new OutlineItem("Root Item");
            var outlineItem2 = new OutlineItem("Item 1");
            var outlineItem3 = new OutlineItem("Item 2");

            outlineItem1.Items.Add(outlineItem2);
            outlineItem1.Items.Add(outlineItem3);

            var clipboardAdapter = new OutlineItemClipboardAdapter();
            clipboardAdapter.SetItem(outlineItem1);

            var item = clipboardAdapter.GetItem();
            Assert.AreEqual(outlineItem1.ToXmlElement().ToString(), item.ToXmlElement().ToString());
        }
    }

    [TestClass]
    public class OutlineItemDecodingMethodTest
    {
        [TestMethod]
        public void CreateFromXmlElementThrowsExceptionWhenExtraAttributesAreSupplied()
        {
            const string xml = "<Item Name=\"Outline Item\" Index=\"1\"/>";

            var xmlElement = XElement.Parse(xml);
            Action createItemFromXmlElementAction = () => OutlineItem.CreateFromXmlElement(xmlElement);

            Assert.ThrowsException<OutlineDecodingException>(createItemFromXmlElementAction);
        }

        [TestMethod]
        public void CreateFromXmlElementThrowsNoExceptionWhenNoExtraAttributesAreSupplied()
        {
            const string xml = "<Item Name=\"Outline Item\"/>";

            var xmlElement = XElement.Parse(xml);

            try
            {
                OutlineItem.CreateFromXmlElement(xmlElement);
            }
            catch (OutlineDecodingException)
            {
                Assert.Fail();
            }
        }
        

        [TestMethod]
        public void CreateFromXmlElementThrowsExceptionWhenItemElementHasWrongName()
        {
            const string xml = "<SomeItem Name=\"Root Item\"/>";

            var xmlElement = XElement.Parse(xml);

            Action action = () => OutlineItem.CreateFromXmlElement(xmlElement);

            Assert.ThrowsException<OutlineDecodingException>(action);
        }

        [TestMethod]
        public void CreateFromXmlElementThrowsExceptionWhenElementItemContainsNoNameAttribute()
        {
            const string xml = "<Item/>";

            var xmlElement = XElement.Parse(xml);

            Action action = () => OutlineItem.CreateFromXmlElement(xmlElement);

            Assert.ThrowsException<OutlineDecodingException>(action);
        }
    }
}
