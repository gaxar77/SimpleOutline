using System;
using System.Windows.Input;

namespace SimpleOutline.ViewModels
{
    public class CommandBase : ICommand
    {
        public ViewModel1 ViewModel { get; protected set; }

        public event EventHandler CanExecuteChanged;

        public CommandBase(ViewModel1 viewModel)
        {
            ViewModel = viewModel;
        }
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {

        }
        public void InvalidateCanExecute()
        {
            var eventArgs = new EventArgs();

            CanExecuteChanged?.Invoke(this, eventArgs);
        }
    }
}
