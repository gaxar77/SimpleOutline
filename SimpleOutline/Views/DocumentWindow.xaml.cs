using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SimpleOutline.ViewModels;
using SimpleOutline.Models;

namespace SimpleOutline.Views
{
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        public DocumentWindow()
        {
            InitializeComponent();
        }

        private void outlineTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = (ViewModel1)DataContext;

            viewModel.SelectedItem = (OutlineItem)e.NewValue;
        }

        private void TreeViewItem_Unselected(object sender, RoutedEventArgs e)
        {
            /*
            var treeViewItem = (TreeViewItem)sender;
            var editField = (TextBox)treeViewItem.

            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(editField), null);
            Keyboard.ClearFocus();*/
        }

        private void TreeViewItem_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = (TextBox)sender;
                FocusManager.SetFocusedElement(textBox, null);

                Keyboard.ClearFocus();

                outlineTreeView.Focus();
                textBox.InvalidateProperty(TextBox.TextProperty);
            }
        }

        private void TreeViewItem_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = (ViewModel1)DataContext;
            var textBox = (TextBox)sender;
            var outlineItem = (OutlineItem)textBox.DataContext;
            viewModel.RenameItem(outlineItem, textBox.Text);
        }

        private void TreeViewItem_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var outlineItem = (OutlineItem)textBox.DataContext;

            outlineItem.IsSelectedInView = true;
        }

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel1)DataContext;
            var menuItem = (MenuItem)sender;

            switch(menuItem.Header)
            {
                case "Cut":
                    viewModel.CutCommand.Execute(null);
                    break;
                case "Copy":
                    viewModel.CopyCommand.Execute(null);
                    break;
                case "Paste":
                    viewModel.PasteCommand.Execute(null);
                    break;
                case "Delete":
                    viewModel.DeleteItemCommand.Execute(null);
                    break;
                case "Insert":
                    viewModel.InsertItemCommand.Execute(null);
                    break;
                case "Move Up":
                    viewModel.MoveItemCommand.Execute(-1);
                    break;
                case "Move Down":
                    viewModel.MoveItemCommand.Execute(1);
                    break;
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var menu = (ContextMenu)sender;
            var outlineItem = (OutlineItem)menu.DataContext;

            outlineItem.IsSelectedInView = true;
        }
    }
}
