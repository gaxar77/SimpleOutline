using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
    public class InsertItemCommand : UndoableCommand
    {
        const string DefaultCommandParameter = "InsertBefore";

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

            var commandParameter = (string)parameter;
            if (commandParameter == null)
                commandParameter = DefaultCommandParameter;

            switch (commandParameter)
            {
                case "InsertBefore":
                case "InsertAfter":
                case "InsertAsFirstChild":
                case "InsertAsNext":
                    break;
                default:
                    throw new CommandFailedException();
            }

            if (ViewModel.SelectedItem.ParentItem == null)
                commandParameter = "InsertAsFirstChild";
            else
            {
                if (commandParameter == "InsertAsNext")
                {
                    if (ViewModel.SelectedItem.Items.Count > 0)
                        commandParameter = "InsertAsFirstChild";
                    else
                        commandParameter = "InsertAfter";
                }
            }

            if (commandParameter == "InsertBefore")
            {
                ExecuteInsertItemBefore();
            }
            else if (commandParameter == "InsertAfter")
            {
                ExecuteInsertItemAfter();
            }
            else if (commandParameter == "InsertAsFirstChild")
            {
                ExecuteInsertItemAsFirstChild();
            }

            _parentItem = _insertedItem.ParentItem;
            _insertedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }

        private void ExecuteInsertItemAsFirstChild()
        {
            ViewModel.SelectedItem.Items.Insert(0, _insertedItem);
            _parentItem = _insertedItem.ParentItem;
        }

        private void ExecuteInsertItemAfter()
        {
            _parentItem = ViewModel.SelectedItem.ParentItem;
            _parentItem.Items.Insert(ViewModel.SelectedItem.IndexInParent() + 1, _insertedItem);
        }

        private void ExecuteInsertItemBefore()
        {
            _parentItem = ViewModel.SelectedItem.ParentItem;
            _parentItem.Items.Insert(ViewModel.SelectedItem.IndexInParent(), _insertedItem);
        }

        public override void Undo()
        {
            _parentItem.Items.Remove(_insertedItem);
            _lastSelectedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
