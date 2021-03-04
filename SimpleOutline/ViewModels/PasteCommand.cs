using System.Xml.Linq;
using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
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
}
