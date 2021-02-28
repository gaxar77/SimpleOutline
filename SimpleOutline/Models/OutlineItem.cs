using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SimpleOutline.Attributes;

namespace SimpleOutline.Models
{
    [Todo("Replace with custom collection class that rejects null and duplicate items.")]
    class OutlineItemCollection : ObservableCollection<OutlineItem>
    {
    }

    [UnusedCode, UnfinishedCode, UntestedCode]
    class OutlineItem : INotifyPropertyChanged
    {
        private string _name;
        private OutlineItemCollection _items;

        public string Name
        {
            get { return _name; }
            
            set
            {
                if (_name == null)
                    throw new ArgumentNullException(nameof(Name));

                _name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        public OutlineItemCollection Items
        {
            get { return _items; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OutlineItem(string name)
        {
            if (_name == null)
                throw new ArgumentNullException(nameof(name));

            _name = name;
        }
        public void OnPropertyChanged(string propertyName)
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
