using System.Windows.Forms;

namespace SimpleOutline
{
    public static class TreeNodeHelper
    {
        public static TreeNode GetNodeWithTag(this TreeNodeCollection treeNodes, object tag)
        {
            foreach (var treeNode in treeNodes)
            {
                var treeNodeAsTreeNode = (TreeNode)treeNode;
                if (treeNodeAsTreeNode.Tag == tag)
                {
                    return treeNodeAsTreeNode;
                }

                var childOfTreeNode = treeNodeAsTreeNode.Nodes.GetNodeWithTag(tag);
                if (childOfTreeNode != null) return childOfTreeNode;
            }

            return null;
        }
    }
}
