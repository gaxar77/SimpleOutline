using System;
using System.Collections.Generic;
using SimpleOutline.Misc;

namespace SimpleOutline.ViewModels
{
    public class UndoableCommandManager : NotifyableBase
    {
        private Stack<UndoableCommand> _commands;
        private Stack<object> _commandParameters;

        private Stack<UndoableCommand> _redoCommands;
        private Stack<object> _redoCommandParameters;

        private ViewModel1 _viewModel;

        public UndoableCommandManager(ViewModel1 viewModel)
        {
            _commands = new Stack<UndoableCommand>();
            _commandParameters = new Stack<object>();

            _redoCommands = new Stack<UndoableCommand>();
            _redoCommandParameters = new Stack<object>();

            _viewModel = viewModel;
        }

        //Executes and stores command.
        public void ExecuteCommand(UndoableCommand command, object parameter, bool clearRedo = true)
        {
            try
            {
                command.Execute(parameter);

                _commands.Push(command);
                _commandParameters.Push(parameter);

                OnPropertyChanged(nameof(CanUndo));

                if (clearRedo)
                {
                    _redoCommands.Clear();
                    _redoCommandParameters.Clear();
                }
            }
            catch (CommandFailedException)
            {

            }
        }

        public void Undo()
        {
            var command = _commands.Pop();
            var commandParameter = _commandParameters.Pop();

            command.Undo();

            //var redoableCommand = (UndoableCommand)Activator.CreateInstance(command.GetType(), _viewModel);

            _redoCommands.Push(command);
            _redoCommandParameters.Push(commandParameter);

            OnPropertyChanged(nameof(CanUndo));
        }

        public void Redo()
        {
            var command = _redoCommands.Pop();
            var commandParameter = _redoCommandParameters.Pop();

            throw new NotImplementedException();
        }
        public bool CanUndo
        {
            get { return _commands.Count > 0; }
        }
    }
}
