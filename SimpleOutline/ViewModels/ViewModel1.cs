using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows;
using System.Diagnostics;
using SimpleOutline.Models;
using System.Windows.Input;

namespace SimpleOutline.ViewModels
{
    public class ViewModel1
    {
        public OutlineDocument Document { get; private set; }
        public ICommand NewDocumentCommand { get; private set; }
        public ViewModel1()
        {
            NewDocumentCommand = new NewDocumentCommand(this);
        }
           
        public void LoadDocument(string fileName)
        {
            Document = OutlineDocument.Load(fileName);
        }

        public void LoadEmptyDocument()
        {
            Document = new OutlineDocument();
        }
    }

    public class NewDocumentCommand : CommandBase
    {
        public NewDocumentCommand(ViewModel1 viewModel)
        {
            this.ViewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Process.Start("SimpleOutline.exe");
        }
    }

    public abstract class CommandBase : ICommand
    {
        public ViewModel1 ViewModel { get; protected set; }
        
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public void OnPropertyChanged()
        {
            var eventArgs = new EventArgs();

            CanExecuteChanged?.Invoke(this, eventArgs);
        }
    }
}
