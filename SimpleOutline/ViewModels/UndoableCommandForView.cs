using System;

namespace SimpleOutline.ViewModels
{
    public class UndoableCommandForView<T> : CommandBase
        where T : UndoableCommand
    {
        UndoableCommand _command;
        public UndoableCommandForView(ViewModel1 viewModel)
            : base(viewModel)
        {
            _command = CreateUndoableCommand();
        }

        public override bool CanExecute(object parameter)
        {
            return _command.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            var command = CreateUndoableCommand();
            if (command.CanExecute(parameter))
            {
                ViewModel.UndoCommandManager.ExecuteCommand(command, parameter);
            }
        }

        private UndoableCommand CreateUndoableCommand()
        {
            return (UndoableCommand)Activator.CreateInstance(typeof(T), ViewModel);
        }

    }
}
