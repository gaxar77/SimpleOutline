using System;

namespace SimpleOutline.Attributes
{
    class TodoAttribute : Attribute
    {
        private string _todoComment;

        public string TodoComment
        {
            get { return _todoComment; }
        }
        public TodoAttribute(string todoComment)
        {
            if (_todoComment == null)
                throw new ArgumentNullException(nameof(todoComment));
        }
    }
}
