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
        
        public void AddItem(OutlineItem parentItem, OutlineItem newItem)
        {
            TreeNode node = new TreeNode(newItem.Name);
            node.Tag = newItem;

            var parentNode = OutlineTreeView.Nodes.GetNodeWithTag(parentItem);
            if (parentNode == null)
            {
                Document.RootItem = newItem;
                OutlineTreeView.Nodes.Add(node);
            }
            else
            {
                parentItem.ChildItems.Add(newItem);
                parentNode.Nodes.Add(node);
            }

        }
    }
}
