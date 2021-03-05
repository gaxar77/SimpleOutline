using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class MoveItemOutCommand : UndoableCommand
    {
        private int _movedItemOldIndex;
        private OutlineItem _movedItem;
        private OutlineItem _movedItemNewParent;
        private OutlineItem _movedItemOldParent;
        public MoveItemOutCommand(ViewModel1 viewModel)
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

            var parentOfParentOfItemToMove = parentOfItemToMove.ParentItem;
            if (parentOfParentOfItemToMove == null)
                throw new CommandFailedException();

            var itemNewIndex = parentOfParentOfItemToMove.Items.IndexOf(parentOfItemToMove) + 1;

            parentOfItemToMove.Items.RemoveAt(itemOldIndex);
            parentOfParentOfItemToMove.Items.Insert(itemNewIndex, itemToMove);

            _movedItem = itemToMove;
            _movedItemNewParent = parentOfParentOfItemToMove;
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
