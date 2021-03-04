using System;
using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
    public class CommandFailedException : Exception
    {

    }
    public class InsertItemCommand : UndoableCommand
    {
        OutlineItem _parentItem;
        OutlineItem _insertedItem;
        public InsertItemCommand(ViewModel1 viewModel)
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
