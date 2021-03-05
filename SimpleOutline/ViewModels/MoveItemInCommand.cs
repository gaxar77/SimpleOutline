using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class MoveItemInCommand : UndoableCommand
    {
        private int _movedItemOldIndex;
        private OutlineItem _movedItem;
        private OutlineItem _movedItemNewParent;
        private OutlineItem _movedItemOldParent;
        public MoveItemInCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            var itemToMove = ViewModel.SelectedItem;
            if (itemToMove == null)
                throw new CommandFailedException();

            var parentOfItemToMove = itemToMove.ParentItem;
            if (parentOfItemToMove == null)
                throw new CommandFailedException();

            var itemOldIndex = parentOfItemToMove.Items.IndexOf(itemToMove);

            if (itemOldIndex == 0)
                throw new CommandFailedException();

            var itemNewParent = parentOfItemToMove.Items[itemOldIndex - 1];

            parentOfItemToMove.Items.RemoveAt(itemOldIndex);
            itemNewParent.Items.Add(itemToMove);

            _movedItem = itemToMove;
            _movedItemNewParent = itemNewParent;
            _movedItemOldParent = parentOfItemToMove;
            _movedItemOldIndex = itemOldIndex;

            _movedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }

        public override void Undo()
        {
            _movedItemNewParent.Items.Remove(_movedItem);
            _movedItemOldParent.Items.Insert(_movedItemOldIndex, _movedItem);

            _movedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
