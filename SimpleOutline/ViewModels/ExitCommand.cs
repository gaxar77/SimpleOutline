using System;
using System.Windows;

namespace SimpleOutline.ViewModels
{
    //Todo: Implementing prompting of user to save on exit only when document is dirty.
    public class SaveDocumentCommandParams
    {
        public Action SucceedAction { get; set; } = () => { };
    }
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

            var commandParams = new SaveDocumentCommandParams()
            {
                SucceedAction = () => System.Windows.Application.Current.Shutdown()
            };

            switch (dialogResult)
            {
                case MessageBoxResult.Yes:
                    new SaveDocumentCommand(ViewModel).Execute(commandParams);
                    return;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    return;
            }

            System.Windows.Application.Current.Shutdown();
        }
    }
}
