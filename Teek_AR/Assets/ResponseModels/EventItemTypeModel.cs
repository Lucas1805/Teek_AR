using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class EventItemTypeModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Template { get; set; }
        public int Position { get; set; }
        public bool ShowOnContentPage { get; set; }
        public bool Active { get; set; }
    }
}
