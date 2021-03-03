namespace SimpleOutline.ViewModels
{
    public class UndoCommand : CommandBase
    {
        public UndoCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            ViewModel.CommandManager.UndoLastCommand();
        }
    }
}
