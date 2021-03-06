using System;
using System.Web.UI;
using SimpleOutline.Models;
using System.IO;

namespace SimpleOutline.FileFormats
{
    public class OutlineDocumentToHtmlFileExporter
    {
        OutlineDocument _document;
        string _fileName;
        public OutlineDocumentToHtmlFileExporter(OutlineDocument document, string fileName)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            _document = document;

            _fileName = fileName;
        }

        public void Export()
        {
            using (var writer = new StreamWriter(_fileName))
            {
                var htmlWriter = new HtmlTextWriter(writer);
                var outlineDocumentToHtmlWriter = new OutlineDocumentToHtmlWriter(htmlWriter, _document);

                outlineDocumentToHtmlWriter.WriteDocument();
            }
        }
    }
}
