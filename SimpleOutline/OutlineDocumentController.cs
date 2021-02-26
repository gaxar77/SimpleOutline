//Todo: Fix problem with insertion, copying, pasting of items.

using System.Windows.Forms;
using System.Xml.Linq;

namespace EasyOutline
{
    public class OutlineDocumentController
    {
        public TreeView OutlineTreeView { get; set; }
        public OutlineDocument Document { get; set; }

        public OutlineDocumentController(TreeView treeView, OutlineDocument document)
        {
            OutlineTreeView = treeView;
            Document = document;
        }

        public OutlineItem GetSelectedOutlineItem()
        {
            return OutlineTreeView.SelectedNode?.Tag as OutlineItem;
        }

        //Todo: Refactor method
        public void InsertItem(OutlineItem parentItem, OutlineItem newItem, bool insertIntoModel = true)
        {
            TreeNode node = new TreeNode(newItem.Name);
            node.Tag = newItem;

            if (parentItem == null)
            {
                if (OutlineTreeView.Nodes.Count > 0)
                    return;

                Document.RootItem = newItem;
                OutlineTreeView.Nodes.Add(node);
            }
            else
            {
                var parentNode = OutlineTreeView.Nodes.GetNodeWithTag(parentItem);

                if (insertIntoModel)
                {
                    newItem.ParentItem = parentItem;
                    parentItem.Items.Add(newItem);
                }
                
                parentNode.Nodes.Add(node);
            }

            foreach (OutlineItem childItem in newItem.Items.ToArray())
            {
                InsertItem(newItem, childItem, false);
            }
        }

        public void RemoveSelectedItem()
        {
            var selectedItem = GetSelectedOutlineItem();
            RemoveItem(selectedItem);
        }
        public void RemoveItem(OutlineItem item)
        {
            if (item.ParentItem == null)
                throw new System.ApplicationException();

            item.ParentItem.Items.Remove(item);

            TreeNode node = OutlineTreeView.Nodes.GetNodeWithTag(item);
            node.Parent.Nodes.Remove(node);
        }

        public void RenameItem(OutlineItem item, string newName)
        {
            item.Name = newName;

            TreeNode node = OutlineTreeView.Nodes.GetNodeWithTag(item);
            node.Name = item.Name;
        }

        public void CreateView()
        {
            TreeView outlineDocumentTreeView = new TreeView();
            outlineDocumentTreeView.Dock = DockStyle.Fill;
            outlineDocumentTreeView.LabelEdit = true;
            outlineDocumentTreeView.AfterLabelEdit += (s, e) =>
            {
                var outlineItem = (OutlineItem)e.Node.Tag;
                outlineItem.Name = e.Label;
            };

            OutlineTreeView = outlineDocumentTreeView;
        }

        public void LoadView()
        {
            if (Document.RootItem != null)
            {
                LoadItem(Document.RootItem);
            }
        }

        public void LoadItem(OutlineItem item)
        {
            InsertItem(item.ParentItem, item);
            foreach (OutlineItem childItem in item.Items)
            {
                InsertItem(item, childItem);
            }
        }

        public void CopySelectedItem()
        {
            var selectedItem = GetSelectedOutlineItem();
            var selectedItemXElement = selectedItem.ToXElement();
            var selectedItemXml = selectedItemXElement.ToString();

            Clipboard.SetText(selectedItemXml, TextDataFormat.UnicodeText);
        }

        public void PasteItem()
        {
            var xmlInClipboard = Clipboard.GetText();
            var xElementOfXmlFromClipboard = XElement.Parse(xmlInClipboard);
            var newItem = new OutlineItem(xElementOfXmlFromClipboard);

            var selectedItem = GetSelectedOutlineItem();

            InsertItem(selectedItem, newItem);
        }

        public void CutSelectedItem()
        {
            CopySelectedItem();
            RemoveSelectedItem();
        }
    }
}
