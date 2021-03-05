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

            //return ViewModel.CommandManager.CanUndo;
        }

        public override void Execute(object parameter)
        {
            if (ViewModel.UndoCommandManager.CanUndo)
            {
                ViewModel.UndoCommandManager.Undo();
            }
        }
    }
}
