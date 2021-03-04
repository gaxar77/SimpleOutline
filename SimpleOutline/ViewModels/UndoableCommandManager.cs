using System.Collections.Generic;
using SimpleOutline.Misc;

namespace SimpleOutline.ViewModels
{
    public class UndoableCommandManager : NotifyableBase
    {
        private Stack<UndoableCommand> _commands;
        private Stack<object> _commandParameters;
        private ViewModel1 _viewModel;

        public UndoableCommandManager(ViewModel1 viewModel)
        {
            _commands = new Stack<UndoableCommand>();
            _commandParameters = new Stack<object>();
            _viewModel = viewModel;
        }

        //Executes and stores command.
        public void ExecuteCommand(UndoableCommand command, object parameter)
        {
            try
            {
                command.Execute(parameter);

                _commands.Push(command);
                _commandParameters.Push(parameter);

                OnPropertyChanged(nameof(CanUndo));
            }
            catch (CommandFailedException)
            {

            }
        }

        public void UndoLastCommand()
        {
            var command = _commands.Pop();
            var commandParameter = _commandParameters.Pop();

            command.Undo();

            OnPropertyChanged(nameof(CanUndo));
        }

        public bool CanUndo
        {
            get { return _commands.Count > 0; }
        }
    }
}
