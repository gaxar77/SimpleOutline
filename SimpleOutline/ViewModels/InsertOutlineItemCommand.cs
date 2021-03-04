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

        public override void Undo(object parameter)
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
                ViewModel.Document.Items.Add(_insertedItem);
            }

            var newItemName = TextInputDialog.PromptUser("What do you want to name the item?", "New Outline Item", 
                _insertedItem.Name);

            //Todo: Add code to scroll to/bring into view the newly inserted item.

            if (newItemName != null)
            {
                _insertedItem.Name = newItemName;
            }
        }

        public override void Undo(object parameter)
        {
            if (_parentItem != null)
            {
                _parentItem.Items.Remove(_insertedItem);
            }
            else
            {
                ViewModel.Document.Items.Remove(_insertedItem);
            }
        }
    }
}
