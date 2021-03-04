using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
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

            _insertedItem.IsSelectedInView = true;
            ViewModel.SetFocusOnItemsView();
        }

        public override void Undo()
        {
            _parentItem.Items.Remove(_insertedItem);
            _parentItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
