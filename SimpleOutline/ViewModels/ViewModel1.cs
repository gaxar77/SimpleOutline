using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using SimpleOutline.Models;
using SimpleOutline.Views;
using SimpleOutline.Misc;

namespace SimpleOutline.ViewModels
{
    public class ViewModel1 : NotifyableBase
    {
        private OutlineItem _selectedItem;
        public DocumentWindow _view;
        public DocumentWindow View
        {
            get { return _view; }

            set
            {
                if (_view != value)
                {
                    _view = value;

                    OnPropertyChanged(nameof(View));
                }
            }
        }
        public OutlineDocument Document { get; private set; }
        public ICommand NewDocumentCommand { get; private set; }
        public ICommand OpenDocumentCommand { get; private set; }
        public ICommand SaveDocumentCommand { get; private set; }
        public ICommand SaveDocumentAsCommand { get; private set; }
        public ICommand InsertItemCommand { get; private set; }
        public ICommand DuplicateItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand MoveItemCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        
        public ICommand MoveInCommand { get; private set; }
        public ICommand MoveOutCommand { get; private set; }
        public UndoableCommandManager UndoCommandManager { get; private set; }

        public OutlineItem SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        public ViewModel1()
        {
            UndoCommandManager = new UndoableCommandManager(this);

            NewDocumentCommand = new NewDocumentCommand(this);
            OpenDocumentCommand = new OpenDocumentCommand(this);
            SaveDocumentCommand = new SaveDocumentCommand(this);
            SaveDocumentAsCommand = new SaveDocumentAsCommand(this);

            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);

            InsertItemCommand = new UndoableCommandForView<InsertItemCommand>(this);
            DuplicateItemCommand = new UndoableCommandForView<DuplicateItemCommand>(this);
            DeleteItemCommand = new UndoableCommandForView<DeleteItemCommand>(this);
            MoveItemCommand = new UndoableCommandForView<MoveItemCommand>(this);
            MoveInCommand = new UndoableCommandForView<MoveItemInCommand>(this);
            MoveOutCommand = new UndoableCommandForView<MoveItemOutCommand>(this);

            CutCommand = new UndoableCommandForView<CutCommand>(this);
            CopyCommand = new CopyCommand(this);
            PasteCommand = new UndoableCommandForView<PasteCommand>(this);

            AboutCommand = new AboutCommand(this);
            ExitCommand = new ExitCommand(this);
        }

        public void LoadDocument(string fileName)
        {
            Document = OutlineDocument.Load(fileName);
        }

        public void LoadNewDocument()
        {
            Document = new OutlineDocument();
        }

        public void SetFocusOnItemsView()
        {
            View.outlineTreeView.Focus();
        }

        public void RenameItem(OutlineItem item, string newName)
        {
            var renameCommand = new RenameItemCommand(this);
            renameCommand.Item = item;
            renameCommand.NewName = newName;

            UndoCommandManager.ExecuteCommand(renameCommand, null);
        }

        public void SelectNextItem()
        {
            if (SelectedItem == null)
            {
                return;
            }


            MessageBox.Show("Next");

            var nextItem = SelectedItem.NextItem();
            if (nextItem != null)
                nextItem.IsSelectedInView = true;

            MessageBox.Show(nextItem.Name, "Next Item");
        }

        public void SelectPreviousItem()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var previousItem = SelectedItem.PreviousItem();
            if (previousItem != null)
                previousItem.IsSelectedInView = true;

        }
    }
}