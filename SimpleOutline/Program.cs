using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SimpleOutline.Models;
using SimpleOutline.Views;

namespace SimpleOutline
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var app = new Application();
            var mainWindow = new DocumentWindow();

            var document = new OutlineDocument();
            

            mainWindow.DataContext = document;
            var item = new OutlineItem("Root");

            item.Items.Add(new OutlineItem("Item 1"));
            item.Items.Add(new OutlineItem("Item 2"));

            document.Items.Add(item);
            app.Run(mainWindow);

        }
    }
}
