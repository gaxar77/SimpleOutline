using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class PasteCommand : UndoableCommand
    {
        OutlineItem _oldItem;
        int _oldItemIndex;
        OutlineItemCollection _oldItemParentCollection;
        public PasteCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
            
        }

        public override void Execute(object parameter)
        {
            var outlineItemFromClipboard = OutlineItem.LoadFromClipboard();
            if (outlineItemFromClipboard == null)
                throw new CommandFailedException();

            if (ViewModel.SelectedItem == null)
                throw new CommandFailedException();

            _oldItem = ViewModel.SelectedItem;

            var parentOfSelectedItem = ViewModel.SelectedItem.ParentItem;
            if (parentOfSelectedItem == null)
            {
                _oldItemParentCollection = ViewModel.Document.Items;
            }
            else
            {
                _oldItemParentCollection = parentOfSelectedItem.Items;
            }

            _oldItemIndex = _oldItemParentCollection.IndexOf(_oldItem);
            _oldItemParentCollection.RemoveAt(_oldItemIndex);
            _oldItemParentCollection.Insert(_oldItemIndex, outlineItemFromClipboard);

            outlineItemFromClipboard.IsSelectedInView = true;
        }

        public override void Undo()
        {
            _oldItemParentCollection.RemoveAt(_oldItemIndex);
            _oldItemParentCollection.Insert(_oldItemIndex, _oldItem);

            _oldItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
