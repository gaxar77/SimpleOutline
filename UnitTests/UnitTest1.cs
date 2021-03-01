using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Data;

namespace UnitTests
{
    [TestClass]
    public class ClipboardDataTests
    {
        [TestMethod]
        public void SaveAndLoadFromClipboardThrowsNoExceptionWhenRightFormatIsRetrieved()
        {
            var text = "This is some data from the SimpleOutline program.";
            var clipboardData = new ClipboardData(text);

            clipboardData.SaveToClipboard();

            try
            {
                var loadedClipboardData = ClipboardData.LoadFromClipboard();
            }
            catch (ClipboardDataUnexpectedFormatException)
            {
                Assert.Fail("ClipboardDataUnexpctedFormatException was caught.");
            }
        }

        [TestMethod]
        public void LoadFromClipboardThrowsExceptionWhenWrongFormatIsRetrieved()
        {
            Clipboard.SetText("Whatever");

            Action action = () => ClipboardData.LoadFromClipboard();

            Assert.ThrowsException<ClipboardDataUnexpectedFormatException>(action);
        }

        [TestMethod]
        public void LoadFromClipboardRetrievesSameDataPutInClipboardBySaveFromClipboard()
        {
            var data = "This is some SimpleOutline program data.";
            var clipboardData = new ClipboardData(data);

            clipboardData.SaveToClipboard();

            var loadedClipboardData = ClipboardData.LoadFromClipboard();

            Assert.AreEqual(data, loadedClipboardData.Data);
        }
    }
    
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

            outlineItem1.CopyToClipboard();

            var outlineItemFromClipboard = OutlineItem.LoadFromClipboard();

            Assert.AreEqual(outlineItem1.ToXmlElement().ToString(), outlineItemFromClipboard.ToXmlElement().ToString());
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
