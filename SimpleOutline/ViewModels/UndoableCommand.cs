using System;
using SimpleOutline.Models;

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

        public virtual void Undo()
        {

        }
    }
    
    public class RenameItemCommand : UndoableCommand
    {
        private string _oldName;
        public OutlineItem Item { get; set; }
        public string NewName { get; set; }
        public RenameItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
            
        }

        public override void Execute(object parameter)
        {
            if (NewName == null)
            {
                throw new InvalidOperationException();
            }

            if (Item == null)
            {
                throw new InvalidOperationException();
            }

            _oldName = Item.Name;
            Item.Name = NewName;

            ViewModel.InvalidateCanExecuteCommands();
        }

        public override void Undo()
        {
            Item.Name = _oldName;
            Item.IsSelectedInView = true;
        }
    }
}
