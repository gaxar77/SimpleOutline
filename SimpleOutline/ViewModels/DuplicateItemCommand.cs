using System.Windows;

namespace SimpleOutline.ViewModels
{
    public class DuplicateItemCommand : UndoableCommand
    {
        //Temporary method of doing duplication.
        UndoableCommand _insertCommand;
        UndoableCommand _pasteCommand;

        public DuplicateItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
            _insertCommand = new InsertItemCommand(viewModel);
            _pasteCommand = new PasteCommand(viewModel);
        }

        //Clipboard data preservation has not been tested yet.
        public override void Execute(object parameter)
        {
            if (ViewModel.SelectedItem == null)
                throw new CommandFailedException();

            var clipboardText = Clipboard.GetText();
            ViewModel.SelectedItem.CopyToClipboard();

            _insertCommand.Execute(null);
            _pasteCommand.Execute(null);

            Clipboard.SetText(clipboardText);
        }

        public override void Undo()
        {
            _pasteCommand.Undo();
            _insertCommand.Undo();
        }
    }
}
