using System.Collections.Generic;
using SimpleOutline.Misc;

namespace SimpleOutline.ViewModels
{
    public class UndoableCommandManager : NotifyableBase
    {
        private Stack<UndoableCommand> _commands;
        private ViewModel1 _viewModel;

        public UndoableCommandManager(ViewModel1 viewModel)
        {
            _commands = new Stack<UndoableCommand>();
            _viewModel = viewModel;
        }

        //Executes and stores command.
        public void ExecuteCommand(UndoableCommand command)
        {
            command.Execute(null);

            _commands.Push(command);

            OnPropertyChanged(nameof(CanUndo));
        }

        public void UndoLastCommand()
        {
            var command = _commands.Pop();

            command.Undo(null);

            OnPropertyChanged(nameof(CanUndo));
        }

        public bool CanUndo
        {
            get { return _commands.Count > 0; }
        }
    }
}
