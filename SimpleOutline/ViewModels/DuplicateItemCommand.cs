using System.Windows;
using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class DuplicateItemCommand : UndoableCommand
    {
        InsertItemCommand _insertCommand;
        
        public DuplicateItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        //Clipboard data preservation has not been tested yet.
        public override void Execute(object parameter)
        {
            if (ViewModel.SelectedItem == null)
                throw new CommandFailedException();

            _insertCommand = new InsertItemCommand(ViewModel);
            _insertCommand.ItemToInsert = ViewModel.SelectedItem;

            _insertCommand.Execute(InsertItemCommand.InsertionMode.InsertBefore);
        }

        public override void Undo()
        {
            _insertCommand.Undo();
        }
    }
}
