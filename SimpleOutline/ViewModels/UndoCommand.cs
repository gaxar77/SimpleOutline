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
            return ViewModel.UndoCommandManager.CanUndo;
        }

        public override void Execute(object parameter)
        {
            if (ViewModel.UndoCommandManager.CanUndo)
            {
                ViewModel.UndoCommandManager.Undo();
            }

            ViewModel.InvalidateCanExecuteCommands();
        }
    }
}
