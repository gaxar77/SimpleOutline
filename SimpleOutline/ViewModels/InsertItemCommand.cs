using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
    public class InsertItemCommand : UndoableCommand
    {
        OutlineItem _parentItem;
        OutlineItem _insertedItem;
        OutlineItem _lastSelectedItem;
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
            if (ViewModel.SelectedItem == null)
                throw new CommandFailedException();

            _insertedItem = new OutlineItem("Topic");

            _lastSelectedItem = ViewModel.SelectedItem;
            var indexOfSelectedItem = _lastSelectedItem.IndexInParent();
            if (indexOfSelectedItem == -1)
            {
                ViewModel.Document.Items[0].Items.Insert(0, _insertedItem);
            }
            else
            {
                _lastSelectedItem.ParentItem.Items.Insert(indexOfSelectedItem + 1, _insertedItem);
            }

            _parentItem = _insertedItem.ParentItem;
            _insertedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }

        public override void Undo()
        {
            _parentItem.Items.Remove(_insertedItem);
            _lastSelectedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
