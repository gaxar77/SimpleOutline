using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EasyOutline
{
    public class OutlineDocumentsController
    {
        public TabControl DocumentsTabControl { get; set; }
        public OutlineDocuments Documents { get; set; }

        public OutlineDocumentsController(TabControl documentsControl, OutlineDocuments documents)
        {
            DocumentsTabControl = documentsControl;
            Documents = documents;
        }

        public void AddDocument(OutlineDocument document)
        {
            Documents.Documents.Add(document);

            var page = new TabPage(document.FileName);
            var documentController = new OutlineDocumentController(null, document);
            page.Tag = documentController;

            documentController.CreateView();
            documentController.LoadView();

            page.Controls.Add(documentController.OutlineTreeView);

            DocumentsTabControl.TabPages.Add(page);
        }

        public OutlineDocument GetSelectedDocument()
        {
            var documentController = (OutlineDocumentController)DocumentsTabControl.SelectedTab.Tag;

            return documentController.Document;
        }

        public OutlineDocumentController GetDocumentController(OutlineDocument document)
        {
            foreach (TabPage page in this.DocumentsTabControl.TabPages)
            {
                var documentController = (OutlineDocumentController)page.Tag;
                if (documentController.Document == document)
                    return documentController;
            }

            throw new System.Exception();
        }
    }
}
