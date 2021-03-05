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

            var indexOfItemToDelete = parentOfItemToDelete.Items.IndexOf(itemToDelete);

            ViewModel.SelectPreviousItem();
            parentOfItemToDelete.Items.Remove(itemToDelete);

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
