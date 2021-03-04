using System;
using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class MoveItemCommand : UndoableCommand
    {
        private int _movedItemNewIndex;
        private int _movedItemOldIndex;
        private OutlineItem _movedItem;
        private OutlineItem _movedItemParent;

        MoveItemCommand _undoCommand;
        public MoveItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            int bySteps = int.Parse((string)parameter);

            var itemToMove = ViewModel.SelectedItem;
            if (itemToMove == null)
                throw new CommandFailedException();

            var parentOfItemToMove = itemToMove.ParentItem;
            if (parentOfItemToMove == null)
                throw new CommandFailedException();

            var itemOldIndex = parentOfItemToMove.Items.IndexOf(itemToMove);
            var itemNewIndex = itemOldIndex + bySteps;

            if (itemNewIndex >= parentOfItemToMove.Items.Count ||
                itemNewIndex < 0)
                throw new CommandFailedException();

            parentOfItemToMove.Items.Move(itemOldIndex, itemNewIndex);

            _movedItem = itemToMove;
            _movedItemParent = parentOfItemToMove;
            _movedItemNewIndex = itemNewIndex;
            _movedItemOldIndex = itemOldIndex;
        }

        public override void Undo()
        {
            _movedItemParent.Items.Move(_movedItemNewIndex, _movedItemOldIndex);
        }
    }
}
