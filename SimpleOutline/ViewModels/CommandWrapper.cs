namespace SimpleOutline.ViewModels
{
    public class CommandWrapper : CommandBase 
    {
        CommandBase _command;
        public CommandWrapper(ViewModel1 viewModel, CommandBase command)
            : base(viewModel)
        {
            _command = command;
        }

        public override bool CanExecute(object parameter)
        {
            return _command.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _command.Execute(parameter);

            //ViewModel.InvalidateCanExecuteCommands();
        }
    }
}
