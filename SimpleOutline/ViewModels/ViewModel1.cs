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
    //Todo: Implement necessary expansion and updating of selection of items in the view upon the execution of every item command.
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
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand RenameItemCommand { get; private set; }

        public ICommand MoveItemCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public UndoableCommandManager CommandManager { get; private set; }

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
            CommandManager = new UndoableCommandManager(this);

            NewDocumentCommand = new NewDocumentCommand(this);
            OpenDocumentCommand = new OpenDocumentCommand(this);
            SaveDocumentCommand = new SaveDocumentCommand(this);
            SaveDocumentAsCommand = new SaveDocumentAsCommand(this);

            UndoCommand = new UndoCommand(this);

            InsertItemCommand = new UndoableCommandForView<InsertOutlineItemCommand>(this);
            DeleteItemCommand = new UndoableCommandForView<DeleteItemCommand>(this);
            RenameItemCommand = new UndoableCommandForView<RenameOutlineItemCommand>(this);
            MoveItemCommand = new UndoableCommandForView<MoveItemCommand>(this);

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
    }
}