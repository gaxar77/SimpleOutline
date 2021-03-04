using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline.ViewModels
{
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
}
