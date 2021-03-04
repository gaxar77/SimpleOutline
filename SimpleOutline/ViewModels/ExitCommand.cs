using System.Windows;

namespace SimpleOutline.ViewModels
{
    public class ExitCommand : CommandBase
    {
        public ExitCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var dialogResult = System.Windows.MessageBox.Show("Do you want to save this document before exiting?",
                "Exit", System.Windows.MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch(dialogResult)
            {
                case MessageBoxResult.Yes:
                    new SaveDocumentCommand(ViewModel).Execute(null);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    return;
            }

            System.Windows.Application.Current.Shutdown();
        }
    }
}
