using System;
using System.Windows;
using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class SaveDocumentCommand : CommandBase
    {
        public SaveDocumentCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            try
            {
                if (ViewModel.Document.State != OutlineDocumentState.NewDocument)
                {
                    ViewModel.Document.Save();
                }
                else
                {
                    ViewModel.SaveDocumentAsCommand.Execute(null);
                }
            }
            catch (Exception)
            {
                var errorMessage = $"Unable to save the file: {ViewModel.Document.FileName}";

                System.Windows.MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
