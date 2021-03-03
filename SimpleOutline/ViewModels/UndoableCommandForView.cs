using System;

namespace SimpleOutline.ViewModels
{
    public class UndoableCommandForView<T> : CommandBase
        where T : UndoableCommand
    {
        public UndoableCommandForView(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var command = (UndoableCommand)Activator.CreateInstance(typeof(T), ViewModel);
            ViewModel.CommandManager.ExecuteCommand(command);
        }
    }
}
