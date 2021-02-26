using System.Windows.Forms;

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
        
        public void InsertItem(OutlineItem parentItem, OutlineItem newItem)
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

                newItem.ParentItem = parentItem;
                parentItem.Items.Add(newItem);
                parentNode.Nodes.Add(node);
            }
        }

        public void RemoveItem(OutlineItem item)
        {
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
    }
}
