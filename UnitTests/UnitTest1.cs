using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using SimpleOutline.Models;

namespace UnitTests
{
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
