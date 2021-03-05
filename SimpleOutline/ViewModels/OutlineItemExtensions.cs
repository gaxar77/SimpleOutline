using SimpleOutline.Models;
using System;

namespace SimpleOutline.ViewModels
{
    public static class OutlineItemExtensions
    {
        public static int IndexInParent(this OutlineItem item)
        {
            if (item.ParentItem != null)
            {
                return item.ParentItem.Items.IndexOf(item);
            }

            return -1;
        }

        public static bool IsLastChildOfParent(this OutlineItem item)
        {
            var indexInParent = item.IndexInParent();

            if (indexInParent != -1)
            {
                if (item.ParentItem.Items.Count == indexInParent + 1)
                    return true;
            }

            throw new InvalidOperationException();
        }

        public static bool IsFirstChildOfParent(this OutlineItem item)
        {
            if (item.IndexInParent() == 0)
                return true;

            if (item.IndexInParent() == -1)
                throw new InvalidOperationException();

            return false;
        }
        public static OutlineItem NextSibling(this OutlineItem item)
        {
            var indexInParent = item.IndexInParent();
            if (indexInParent == -1)
                return null;

            if (!item.IsLastChildOfParent())
            {
                return item.ParentItem.Items[indexInParent + 1];
            }

            return null;
        }

        public static OutlineItem PreviousSibling(this OutlineItem item)
        {
            var indexInParent = item.IndexInParent();
            if (indexInParent == -1)
                return null;

            if (!item.IsFirstChildOfParent())
            {
                return item.ParentItem.Items[indexInParent - 1];
            }

            return null;
        }

        public static OutlineItem PreviousItem(this OutlineItem item)
        {
            var prevSibling = item.PreviousSibling();
            if (prevSibling != null)
                return prevSibling;

            return item.ParentItem;
        }

        public static OutlineItem NextItem(this OutlineItem item)
        {
            if (item.Items.Count != 0)
                return item.Items[0];

            var nextSibling = item.NextSibling();
            if (nextSibling != null)
                return nextSibling;

            var parentOfItem = item.ParentItem;
            if (parentOfItem == null)
                return null;

            return parentOfItem.NextSibling();
        }
    }
}