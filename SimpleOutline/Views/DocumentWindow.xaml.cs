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
using System.Timers;

namespace SimpleOutline.Views
{
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        Timer _timer;
        public ViewModel1 ViewModel
        {
            get { return (ViewModel1)DataContext; }
            set { DataContext = value; }
        }
        public DocumentWindow()
        {
            InitializeComponent();

            _timer = new Timer(1000);

            _timer.Elapsed += _timer_Elapsed;

            this.Loaded += DocumentWindow_Loaded;
            this.Unloaded += DocumentWindow_Unloaded;
            this.Closing += DocumentWindow_Closing;
        }

        private void DocumentWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void DocumentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            Dispatcher.InvokeAsync(() => ViewModel.ExitCommand.Execute(null));
        }

        private void DocumentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            _timer.Start();
            
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => ViewModel.InvalidateCanExecuteCommands());
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
        private void TreeViewItem_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            //var treeViewItem = (TreeViewItem)FindContainingTreeViewItem(textBox);
            var outlineItem = (OutlineItem)textBox.DataContext;

            outlineItem.IsSelectedInView = true;
            //treeViewItem.IsSelected = true;
        }

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel1)DataContext;
            var menuItem = (MenuItem)sender;

            switch (menuItem.Header)
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
                case "Duplicate":
                    viewModel.DuplicateItemCommand.Execute(null);
                    break;
                case "Delete":
                    viewModel.DeleteItemCommand.Execute(null);
                    break;
                case "Rename":
                    BeginEditSelectedItem();
                    break;
                case "Insert":
                    viewModel.InsertItemCommand.Execute(null);
                    break;
                case "Move Up":
                    viewModel.MoveItemCommand.Execute("-1");
                    break;
                case "Move Down":
                    viewModel.MoveItemCommand.Execute("1");
                    break;
                case "Move In":
                    viewModel.MoveInCommand.Execute(null);
                    break;
                case "Move Out":
                    viewModel.MoveOutCommand.Execute(null);
                    break;
                case "Before":
                    viewModel.InsertItemCommand.Execute("InsertBefore");
                    break;
                case "As Next":
                    viewModel.InsertItemCommand.Execute("InsertAsNext");
                    break;
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var menu = (ContextMenu)sender;
            var outlineItem = (OutlineItem)menu.DataContext;

            outlineItem.IsSelectedInView = true;
        }

        private void outlineTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (ViewModel1)DataContext;

            if (viewModel.SelectedItem == null)
            {
                viewModel.Document.Items[0].IsSelectedInView = true;
                return;
            }

            if (e.Key == Key.F2)
            {
                if (!viewModel.SelectedItem.IsBeingEditedInView)
                    BeginEditSelectedItem();
            }
        }

        private TextBox FindItemEditFieldRecursively(DependencyObject element)
        {
            if (element is TextBox textBox && textBox.Name == "itemEditField")
                return textBox;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var childElement = FindItemEditFieldRecursively(VisualTreeHelper.GetChild(element, i));
                if (childElement != null)
                    return childElement;
            }

            return null;
        }

        private TreeViewItem FindContainingTreeViewItem(DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            if (parent is TreeViewItem treeViewItem)
                return treeViewItem;

            return FindContainingTreeViewItem(parent);
        }

        private void outlineItemEditField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = (ViewModel1)DataContext;
                var textBox = (TextBox)sender;
                var outlineItem = (OutlineItem)textBox.DataContext;
                viewModel.RenameItem(outlineItem, textBox.Text);
                outlineItem.IsBeingEditedInView = false;

                e.Handled = true;
            }
        }

        private void BeginEditSelectedItem()
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.IsBeingEditedInView = true;
                var treeViewItem = GetSelectedTreeViewItem();
                var editField = FindItemEditFieldRecursively(treeViewItem);

                editField.InvalidateProperty(TextBox.TextProperty);
                editField.Focus();
                editField.SelectAll();
            }
        }

        private TreeViewItem GetSelectedTreeViewItem()
        {
            return GetTreeViewItem(outlineTreeView.ItemContainerGenerator, ((ViewModel1)DataContext).SelectedItem);
        }

        public void BringItemIntoView(OutlineItem item)
        {
            GetTreeViewItem(outlineTreeView.ItemContainerGenerator, item).BringIntoView();
        }

        private TreeViewItem GetTreeViewItem(ItemContainerGenerator generator, OutlineItem outlineItem)
        {
            var retrievedItem = generator.ContainerFromItem(outlineItem);
            if (retrievedItem != null)
                return (TreeViewItem)retrievedItem;

            for (int i = 0; i < generator.Items.Count; i++)
            {
                TreeViewItem childItem = (TreeViewItem)generator.ContainerFromIndex(i);
                retrievedItem = GetTreeViewItem(childItem.ItemContainerGenerator, outlineItem);
                if (retrievedItem != null)
                    return (TreeViewItem)retrievedItem;
            }

            return null;
        }
        private void TreeViewItem_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CancelItemEdit();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelItemEdit();
        }

        private void CancelItemEdit()
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.IsBeingEditedInView = false;
            }
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            //CancelItemEdit();
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BeginEditSelectedItem();
        }
    }
}