namespace SimpleOutline.ViewModels
{
    public class UndoableCommand : CommandBase
    {
        public UndoableCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            base.Execute(parameter);
        }

        public virtual void Undo(object parameter)
        {

        }
    }
}
