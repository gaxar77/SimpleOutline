using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class CutCommand : UndoableCommand
    {
        OutlineItem _cutItem;
        OutlineItem _parentOfCutItem;
        int _previousIndexOfCutItem;
        public CutCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var itemToCut = ViewModel.SelectedItem;
            if (itemToCut == null)
                throw new CommandFailedException();

            var parentOfItemToCut = itemToCut.ParentItem;
            var indexOfItemToCut = parentOfItemToCut.Items.IndexOf(itemToCut);

            itemToCut.CopyToClipboard();

            if (parentOfItemToCut != null)
            {
                parentOfItemToCut.Items.Remove(itemToCut);
            }

            _cutItem = itemToCut;
            _parentOfCutItem = parentOfItemToCut;
            _previousIndexOfCutItem = indexOfItemToCut;
        }

        public override void Undo()
        {
            if (_parentOfCutItem != null)
            {
                _parentOfCutItem.Items.Insert(_previousIndexOfCutItem, _cutItem);
            }
        }
    }
}
