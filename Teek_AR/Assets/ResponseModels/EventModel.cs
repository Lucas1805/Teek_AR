using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class EventModel
    {
        public virtual int Id { get; set; }
        public virtual string CreatorUserID { get; set; }
        public virtual Nullable<int> OrganizerId { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string Address { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual string CreatedTime { get; set; }
        public virtual string SeoName { get; set; }
        public virtual bool IsFree { get; set; }
        public virtual Nullable<double> Price { get; set; }
        public virtual Nullable<int> Status { get; set; }
        public virtual bool EnableInteractionPage { get; set; }
        public virtual bool Active { get; set; }
    }
}
