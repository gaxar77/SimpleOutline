using System.Diagnostics;
using System.Windows.Forms;

namespace SimpleOutline.ViewModels
{
    public class OpenDocumentCommand : CommandBase
    {
        public OpenDocumentCommand(ViewModel1 viewModel)
            : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = SimpleOutlineFileDialogConstants.Filter;
            openFileDialog.DefaultExt = SimpleOutlineFileDialogConstants.DefaultExt;
            //openFileDialog.FileName = String.Empty;

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                Process.Start("SimpleOutline.exe", $"\"{openFileDialog.FileName}\"");
            }
        }
    }
}
