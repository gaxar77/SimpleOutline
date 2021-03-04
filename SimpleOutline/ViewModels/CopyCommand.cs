namespace SimpleOutline.ViewModels
{
    public class CopyCommand : CommandBase
    {
        public CopyCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var selectedItem = ViewModel.SelectedItem;
            if (selectedItem == null)
                throw new CommandFailedException();

            selectedItem.CopyToClipboard();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
