using System.Collections.ObjectModel;
using SimpleOutline.Models;
using System.Windows.Input;
using SimpleOutline.Misc;

namespace SimpleOutline.ViewModels
{
    public class ViewModel1 : NotifyableBase
    {
        private OutlineItem _selectedItem;
        public OutlineDocument Document { get; private set; }
        public ICommand NewDocumentCommand { get; private set; }
        public ICommand OpenDocumentCommand { get; private set; }
        public ICommand SaveDocumentCommand { get; private set; }
        public ICommand SaveDocumentAsCommand { get; private set; }
        public ICommand InsertItemCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
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
        }

        public void LoadDocument(string fileName)
        {
            Document = OutlineDocument.Load(fileName);
        }

        public void LoadEmptyDocument()
        {
            Document = new OutlineDocument();
        }
    }
}
