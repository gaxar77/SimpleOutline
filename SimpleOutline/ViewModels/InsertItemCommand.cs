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
            _lastSelectedItem = ViewModel.SelectedItem;
            _insertedItem = new OutlineItem("New Item");

            if (parameter is string stringParameter && stringParameter == "FromClipboard")
            {
                _insertedItem = OutlineItem.LoadFromClipboard();
                if (_insertedItem == null)
                    throw new CommandFailedException();
            }

            if (ViewModel.ItemInsertionMode == OutlineItemInsertionMode.InsertAsLastChild)
            {
                _parentItem = ViewModel.SelectedItem;

                if (_parentItem != null)
                {
                    _parentItem.Items.Add(_insertedItem);
                }
                else
                {
                    throw new CommandFailedException();
                }
            }
            else if (ViewModel.ItemInsertionMode == OutlineItemInsertionMode.InsertAsFirstChild)
            {
                _parentItem = ViewModel.SelectedItem;

                if (_parentItem != null)
                {
                    _parentItem.Items.Insert(0, _insertedItem);
                }
                else
                {
                    throw new CommandFailedException();
                }
            }
            else if (ViewModel.ItemInsertionMode == OutlineItemInsertionMode.InsertAsNextSibling)
            {
                _parentItem = ViewModel.SelectedItem;

                if (_parentItem != null)
                {
                    _parentItem = _parentItem.ParentItem;
                    if (_parentItem != null)
                    {
                        _parentItem.Items.Insert(_parentItem.Items.IndexOf(ViewModel.SelectedItem) + 1, _insertedItem);
                    }
                    else
                    {
                        throw new CommandFailedException();
                    }
                }
                else
                {
                    throw new CommandFailedException();
                }
            }
            else if (ViewModel.ItemInsertionMode == OutlineItemInsertionMode.InsertAsPreviousSibling)
            {
                _parentItem = ViewModel.SelectedItem;

                if (_parentItem != null)
                {
                    _parentItem = _parentItem.ParentItem;
                    if (_parentItem != null)
                    {
                        _parentItem.Items.Insert(_parentItem.Items.IndexOf(ViewModel.SelectedItem), _insertedItem);
                    }
                    else
                    {
                        throw new CommandFailedException();
                    }
                }
                else
                {
                    throw new CommandFailedException();
                }
            }

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
