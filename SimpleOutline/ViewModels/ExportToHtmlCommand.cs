using System;
using System.Windows.Forms;
using SimpleOutline.FileFormats;

namespace SimpleOutline.ViewModels
{
    public class ExportToHtmlCommand : CommandBase
    {
        public ExportToHtmlCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }
        public override void Execute(object parameter)
        {
            var exportDialog = new SaveFileDialog()
            {
                Filter = "Html Files (*.html)|*.html",
                DefaultExt = ".html",
                Title = "Export to Html",
            };

            var dialogResult = exportDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    var exporter = new OutlineDocumentToHtmlFileExporter(ViewModel.Document, exportDialog.FileName);
                    exporter.Export();
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("SimpleOutline was unable to explort the outline.",
                        "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
    }
}
