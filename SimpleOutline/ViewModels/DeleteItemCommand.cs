using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class DeleteItemCommand : UndoableCommand
    {
        private OutlineItem _deletedItem;
        private OutlineItem _parentOfDeletedItem;
        private int _previousIndexOfDeletedItem;
        public DeleteItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var itemToDelete = ViewModel.SelectedItem;

            if (itemToDelete == null)
                throw new CommandFailedException();

            var parentOfItemToDelete = itemToDelete.ParentItem;
            if (parentOfItemToDelete == null)
                throw new CommandFailedException();

            bool selectionHandled = false;
            int indexOfItemToDelete = itemToDelete.IndexInParent();
            if (indexOfItemToDelete == parentOfItemToDelete.Items.Count - 1)
            {
                ViewModel.SelectPreviousItem();
                selectionHandled = true;
            }

            parentOfItemToDelete.Items.Remove(itemToDelete);

            if (!selectionHandled)
            {
                parentOfItemToDelete.Items[indexOfItemToDelete].IsSelectedInView = true;
            }

            _deletedItem = itemToDelete;
            _parentOfDeletedItem = parentOfItemToDelete;
            _previousIndexOfDeletedItem = indexOfItemToDelete;

            ViewModel.SetFocusOnItemsView();
        }
        public override void Undo()
        {
            _parentOfDeletedItem.Items.Insert(_previousIndexOfDeletedItem, _deletedItem);
            _deletedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
