using System.Windows.Forms;

namespace SimpleOutline.ViewModels
{
    public class SaveDocumentAsCommand : CommandBase
    {
        public SaveDocumentAsCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = SimpleOutlineFileDialogConstants.Filter;
            saveFileDialog.DefaultExt = SimpleOutlineFileDialogConstants.DefaultExt;
            //saveFileDialog.FileName = String.Empty;

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                ViewModel.Document.Save(saveFileDialog.FileName);
            }
        }
    }
}
