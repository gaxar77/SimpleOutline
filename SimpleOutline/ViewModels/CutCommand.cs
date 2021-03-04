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

            itemToCut.CopyToClipboard();
            var parentOfItemToCut = itemToCut.ParentItem;
            if (parentOfItemToCut == null)
                throw new CommandFailedException();

            var indexOfItemToCut = parentOfItemToCut.Items.IndexOf(itemToCut);

            parentOfItemToCut.Items.Remove(itemToCut);

            _cutItem = itemToCut;
            _parentOfCutItem = parentOfItemToCut;
            _previousIndexOfCutItem = indexOfItemToCut;

            if (_previousIndexOfCutItem < _parentOfCutItem.Items.Count)
            {
                _parentOfCutItem.Items[_previousIndexOfCutItem].IsSelectedInView = true;
            }
            else
            {
                if (_parentOfCutItem.Items.Count == 0)
                {
                    _parentOfCutItem.IsSelectedInView = true;
                }
                else
                {
                    var indexOfLastItemInParent = _parentOfCutItem.Items.Count - 1;
                    _parentOfCutItem.Items[indexOfLastItemInParent].IsSelectedInView = true;
                }
            }

            ViewModel.SetFocusOnItemsView();
        }

        public override void Undo()
        {
            if (_parentOfCutItem != null)
            {
                _parentOfCutItem.Items.Insert(_previousIndexOfCutItem, _cutItem);
                _cutItem.IsSelectedInView = true;
            }

            ViewModel.SetFocusOnItemsView();
        }
    }
}
