﻿using System.Xml.Linq;
using SimpleOutline.Models;

namespace SimpleOutline.ViewModels
{
    public class InsertOutlineItemCommand : UndoableCommand
    {
        OutlineItem _parentItem;
        OutlineItem _insertedItem;
        public InsertOutlineItemCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _insertedItem = new OutlineItem("New Item");

            _parentItem = ViewModel.SelectedItem;

            if (_parentItem != null)
            {
                _parentItem.Items.Add(_insertedItem);
            }
            else
            {
                ViewModel.Document.Items.Add(_insertedItem);
            }
        }

        public override void Undo(object parameter)
        {
            if (_parentItem != null)
            {
                _parentItem.Items.Remove(_insertedItem);
            }
            else
            {
                ViewModel.Document.Items.Remove(_insertedItem);
            }
        }
    }
}
