using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using SimpleOutline.Models;
using SimpleOutline.ValueConverters;
using SimpleOutline.Views;
using SimpleOutline.ViewModels;

namespace SimpleOutline
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var app = new Application();

            if (args.Length > 1)
            {
                MessageBox.Show("SimpleOutline only accepts zero or one arguments.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var viewModel = new ViewModel1();
            if (args.Length == 1)
            {
                try
                {
                    viewModel.LoadDocument(args[0]);
                }
                catch (Exception)
                {
                    var errorMessage = $"SimpleOutline could not load the file specified: {args[0]}";

                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            }
            else
            {
                viewModel.LoadNewDocument();
            }

            var mainWindow = new DocumentWindow();
            mainWindow.DataContext = viewModel;
            viewModel.View = mainWindow;

            app.Run(mainWindow);
        }
    }
}
