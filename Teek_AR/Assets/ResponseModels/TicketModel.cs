using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class TicketModel
    {
        public virtual int Id { get; set; }
        public virtual int EventId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Details { get; set; }
        public virtual int TotalQuantity { get; set; }
        public virtual int RemainQuanity { get; set; }
        public virtual double Price { get; set; }
        public virtual bool Active { get; set; }
    }
}
