using System.Windows.Forms;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
    public class InsertItemCommand : UndoableCommand
    {
        public class InsertionMode
        {
            public const string InsertAsFirstChild = "InsertAsFirstChild";
            public const string InsertBefore = "InsertBefore";
            public const string InsertAsNext = "InsertAsNext";
            public const string InsertAfter = "InsertAfter";
        }

        const string DefaultCommandParameter = InsertionMode.InsertBefore;
        
        OutlineItem _parentItem;
        OutlineItem _insertedItem;
        OutlineItem _lastSelectedItem;

        public OutlineItem ItemToInsert { get; set; } = new OutlineItem();

        public InsertItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }
        public override bool CanExecute(object parameter)
        {
            return ViewModel.SelectedItem != null;
        }

        public override void Execute(object parameter)
        {
            if (ViewModel.SelectedItem == null)
                throw new CommandFailedException();

            _lastSelectedItem = ViewModel.SelectedItem;

            var itemToInsert = new OutlineItem();

            string commandParameter = ChooseRightCommandParameter(parameter);
            _insertedItem = InsertItem(commandParameter);
            
            _parentItem = _insertedItem.ParentItem;
            _insertedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }

        private OutlineItem InsertItem(string commandParameter)
        {
            var itemToInsert = ItemToInsert.Clone();

            if (commandParameter == InsertionMode.InsertBefore)
            {
                ExecuteInsertItemBefore(itemToInsert);
            }
            else if (commandParameter == InsertionMode.InsertAfter)
            {
                ExecuteInsertItemAfter(itemToInsert);
            }
            else if (commandParameter == InsertionMode.InsertAsFirstChild)
            {
                ExecuteInsertItemAsFirstChild(itemToInsert);
            }

            return itemToInsert;
        }

        private string ChooseRightCommandParameter(object parameter)
        {
            var commandParameter = (string)parameter;
            if (commandParameter == null)
                commandParameter = DefaultCommandParameter;

            switch (commandParameter)
            {
                case InsertionMode.InsertBefore:
                case InsertionMode.InsertAsFirstChild:
                case InsertionMode.InsertAfter:
                case InsertionMode.InsertAsNext:
                    break;
                default:
                    throw new CommandFailedException();
            }

            if (ViewModel.SelectedItem.ParentItem == null)
                commandParameter = InsertionMode.InsertAsFirstChild;
            else
            {
                if (commandParameter == InsertionMode.InsertAsNext)
                {
                    if (ViewModel.SelectedItem.Items.Count > 0)
                        commandParameter = InsertionMode.InsertAsFirstChild;
                    else
                        commandParameter = InsertionMode.InsertAfter;
                }
            }

            return commandParameter;
        }

        private void ExecuteInsertItemAsFirstChild(OutlineItem itemToInsert)
        {
            ViewModel.SelectedItem.Items.Insert(0, itemToInsert);
        }

        private void ExecuteInsertItemAfter(OutlineItem itemToInsert)
        {
            var parentOfSelectedItem = ViewModel.SelectedItem.ParentItem;
            parentOfSelectedItem.Items.Insert(ViewModel.SelectedItem.IndexInParent() + 1, itemToInsert);
        }

        private void ExecuteInsertItemBefore(OutlineItem itemToInsert)
        {
            var parentOfSelectedItem = ViewModel.SelectedItem.ParentItem;
            parentOfSelectedItem.Items.Insert(ViewModel.SelectedItem.IndexInParent(), itemToInsert);
        }

        public override void Undo()
        {
            _parentItem.Items.Remove(_insertedItem);
            _lastSelectedItem.IsSelectedInView = true;

            ViewModel.SetFocusOnItemsView();
        }
    }
}
