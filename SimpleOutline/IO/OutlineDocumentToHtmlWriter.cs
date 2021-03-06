using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text;
using System.Web.UI;
using SimpleOutline.Models;

namespace SimpleOutline.FileFormats
{
    public class OutlineDocumentToHtmlWriter : IDisposable
    {
        HtmlTextWriter _htmlWriter;

        OutlineDocument _document;
        bool _isDisposed;
        public OutlineDocumentToHtmlWriter(HtmlTextWriter htmlWriter, OutlineDocument document)
        {
            if (htmlWriter == null)
                throw new ArgumentNullException(nameof(htmlWriter));

            _htmlWriter = htmlWriter;
            _document = document;
        }
        public void WriteDocument()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OutlineDocumentToHtmlWriter));

            WriteOutlineDocument(_document);
        }

        private void WriteOutlineRootItem(OutlineItem item)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OutlineDocumentToHtmlWriter));

            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
            _htmlWriter.Write(item.Name);
            _htmlWriter.RenderEndTag();


            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
            foreach (OutlineItem childItem in item.Items)
            {
                WriteOutlineItem(childItem);
            }
            _htmlWriter.RenderEndTag();

            _htmlWriter.Flush();
        }
        private void WriteOutlineItem(OutlineItem item)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OutlineDocumentToHtmlWriter));

            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Li);
            _htmlWriter.Write(item.Name);


            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
            foreach (OutlineItem childItem in item.Items)
            {
                WriteOutlineItem(childItem);
            }
            _htmlWriter.RenderEndTag();

            _htmlWriter.RenderEndTag();
            _htmlWriter.Flush();
        }

        private void WriteOutlineDocument(OutlineDocument document)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OutlineDocumentToHtmlWriter));

            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Html);
            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Head);
            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Title);
            _htmlWriter.RenderEndTag();
            _htmlWriter.RenderEndTag();
            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Body);
            _htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "SimpleOutline_Outline");
            _htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
            WriteOutlineRootItem(document.Items[0]);
            _htmlWriter.RenderEndTag();
            _htmlWriter.RenderEndTag();
            _htmlWriter.RenderEndTag();
            _htmlWriter.Flush();  
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                _htmlWriter.Dispose();

                _isDisposed = true;
            }
        }
    }
}
