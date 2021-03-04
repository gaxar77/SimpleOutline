using System;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{ 
    public class CommandFailedException : Exception
    {

    }

    public class CutCommand : UndoableCommand
    {
        OutlineItem _cutItem;
        OutlineItem _parentOfCutItem;
        int _previousIndexOfCutItem;
        public CutCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var itemToCut = ViewModel.SelectedItem;
            if (itemToCut == null)
                throw new CommandFailedException();

            var parentOfItemToCut = itemToCut.ParentItem;
            var indexOfItemToCut = parentOfItemToCut.Items.IndexOf(itemToCut);

            itemToCut.CopyToClipboard();

            if (parentOfItemToCut != null)
            {
                parentOfItemToCut.Items.Remove(itemToCut);
            }

            _cutItem = itemToCut;
            _parentOfCutItem = parentOfItemToCut;
            _previousIndexOfCutItem = indexOfItemToCut;
        }

        public override void Undo()
        {
            if (_parentOfCutItem != null)
            {
                _parentOfCutItem.Items.Insert(_previousIndexOfCutItem, _cutItem);
            }
        }
    }

    public class CopyCommand : CommandBase
    {
        public CopyCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            ViewModel.SelectedItem.CopyToClipboard();
        }
    }

    public class PasteCommand : UndoableCommand
    {
        private OutlineItem _pastedItem;
        private OutlineItem _parentOfPastedItem;
        public PasteCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var itemToPaste = OutlineItem.LoadFromClipboard();
            if (itemToPaste == null)
            {
                throw new CommandFailedException();
            }

            var selectedItem = ViewModel.SelectedItem;
            if (selectedItem == null)
            {
                throw new CommandFailedException();
            }

            selectedItem.Items.Add(itemToPaste);

            _pastedItem = itemToPaste;
            _parentOfPastedItem = _pastedItem.ParentItem;
        }

        public override void Undo()
        {
            _parentOfPastedItem.Items.Remove(_pastedItem);
        }
    }
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

            try
            {
                var itemOldIndex = parentOfItemToMove.Items.IndexOf(itemToMove);
                var itemNewIndex = itemOldIndex + bySteps;

                parentOfItemToMove.Items.Move(itemOldIndex, itemNewIndex);

                _movedItem = itemToMove;
                _movedItemParent = parentOfItemToMove;
                _movedItemNewIndex = itemNewIndex;
                _movedItemOldIndex = itemOldIndex;
            }
            catch (ApplicationException)
            {
                throw new CommandFailedException();
            }
        }

        public override void Undo()
        {
            _movedItemParent.Items.Move(_movedItemNewIndex, _movedItemOldIndex);
        }
    }

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

            parentOfItemToDelete.Items.Remove(itemToDelete);

            _deletedItem = itemToDelete;
            _parentOfDeletedItem = parentOfItemToDelete;
            _previousIndexOfDeletedItem = indexOfItemToDelete;
        }

        public override void Undo()
        {
            _parentOfDeletedItem.Items.Insert(_previousIndexOfDeletedItem, _deletedItem);
        }
    }

    public class RenameOutlineItemCommand : UndoableCommand
    {
        private OutlineItem _renamedItem;
        private string _oldItemName;
        public RenameOutlineItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
            
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var itemToRename = ViewModel.SelectedItem;
            _oldItemName = itemToRename.Name;

            var newItemName = TextInputDialog.PromptUser("What do you want to name the selected item?", "Rename Outline Item",
                    itemToRename.Name);

            if (newItemName == null)
            {
                throw new CommandFailedException();
            }

            itemToRename.Name = newItemName;

            _renamedItem = itemToRename;
        }

        public override void Undo()
        {
            _renamedItem.Name = _oldItemName;
        }
    }
    public class InsertOutlineItemCommand : UndoableCommand
    {
        OutlineItem _parentItem;
        OutlineItem _insertedItem;
        public InsertOutlineItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _insertedItem = new OutlineItem("New Item");

            _parentItem = ViewModel.SelectedItem;

            if (_parentItem != null)
            {
                _parentItem.Items.Add(_insertedItem);
            }
            else
            {
                throw new CommandFailedException();
            }

            var newItemName = TextInputDialog.PromptUser("What do you want to name the item?", "New Outline Item", 
                _insertedItem.Name);

            //Todo: Add code to scroll to/bring into view the newly inserted item.

            if (newItemName != null)
            {
                _insertedItem.Name = newItemName;
            }
            else
            {
                _parentItem.Items.Remove(_insertedItem);

                throw new CommandFailedException();
            }
        }

        public override void Undo()
        {
            _parentItem.Items.Remove(_insertedItem);
        }
    }
}
