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
            ViewModel.SelectedItem.CopyToClipboard();
        }
    }
}
