using System.Diagnostics;

namespace SimpleOutline.ViewModels
{
    public class NewDocumentCommand : CommandBase
    {
        public NewDocumentCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

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
}
