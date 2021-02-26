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

            TabPage page = new TabPage(document.FileName);
            page.Tag = document;

            TreeView outlineDocumentTreeView = new TreeView();
            outlineDocumentTreeView.Dock = DockStyle.Fill;
            outlineDocumentTreeView.LabelEdit = true;
            outlineDocumentTreeView.AfterLabelEdit += (s, e) =>
            {
                var outlineItem = (OutlineItem)e.Node.Tag;
                outlineItem.Name = e.Label;
            };

            page.Controls.Add(outlineDocumentTreeView);
            DocumentsTabControl.TabPages.Add(page);

            GetDocumentController(document).InsertItem(null, new OutlineItem() { Name = "Outline" });
        }

        public OutlineDocument GetSelectedDocument()
        {
            return DocumentsTabControl.SelectedTab.Tag as OutlineDocument;
        }

        public OutlineDocumentController GetDocumentController(OutlineDocument document)
        {
            TreeView treeView = 
                DocumentsTabControl
                .TabPages
                .OfType<TabPage>()
                .SingleOrDefault(page => page.Tag == document)
                .Controls[0] as TreeView;

            if (treeView.Tag is OutlineDocumentController documentController)
            {
                return documentController;
            }
            else
            {
                treeView.Tag = new OutlineDocumentController(
                    treeView, document);

                return treeView.Tag as OutlineDocumentController;
            }
        }
    }
}
