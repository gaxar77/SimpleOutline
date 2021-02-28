using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SimpleOutline
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

        public void AddNewDocument()
        {
            OutlineDocument document = new OutlineDocument() { FileName = "Untitled.sof" };
            document.RootItem = new OutlineItem("Untitled Outline");

            AddDocument(document);
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

        public void InsertNewOutlineItemIntoSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            var selectedItem = controllerOfSelectedDocument.GetSelectedOutlineItem();
            controllerOfSelectedDocument.InsertItem(selectedItem, new OutlineItem("New Item"));
        }

        public void DeleteSelectedOutlineItemFromSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            controllerOfSelectedDocument.DeleteSelectedItem();
        }

        public OutlineDocumentController GetControllerOfSelectedDocument()
        {
            var selectedDocumentPage = DocumentsTabControl.SelectedTab;
            var controllerOfSelectedDocument = (OutlineDocumentController)selectedDocumentPage.Tag;

            return controllerOfSelectedDocument;
        }

        public void CopySelectedOutlineItemInSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            controllerOfSelectedDocument.CopySelectedItem();
        }

        public void PasteOutlineItemInClipboardInSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            controllerOfSelectedDocument.PasteItem();
        }

        public void CutSelectedOutlineItemInSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            controllerOfSelectedDocument.CutSelectedItem();
        }

        public void RenameSelectedOutlineItemInSelectedDocument()
        {
            var controllerOfSelectedDocument = GetControllerOfSelectedDocument();
            controllerOfSelectedDocument.RenameSelectedItem();
        }
    }
}
