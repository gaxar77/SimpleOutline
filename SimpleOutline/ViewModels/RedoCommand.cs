namespace SimpleOutline.ViewModels
{
    public class RedoCommand : CommandBase
    {
        public RedoCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            ViewModel.UndoCommandManager.Redo();
        }

        public override bool CanExecute(object parameter)
        {
            return false;
        }
    }
}
