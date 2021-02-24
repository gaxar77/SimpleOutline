using System.Collections.Generic;

namespace EasyOutline
{
    public class OutlineItem
    {
        public string Name { get; set; }

        public List<OutlineItem> ChildItems { get; set; } =
            new List<OutlineItem>();

        public OutlineItem ParentItem { get; set; }
    }
}
