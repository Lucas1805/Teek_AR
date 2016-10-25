using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public class StoreModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual Nullable<bool> IsAvailable { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string CreateDate { get; set; }
        public virtual Nullable<System.TimeSpan> OpenTime { get; set; }
        public virtual Nullable<System.TimeSpan> CloseTime { get; set; }
        public virtual Nullable<int> OrganizerId { get; set; }
        public virtual Nullable<bool> Active { get; set; }
    }

    public class MyTimeClass
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
        public long Ticks { get; set; }
        public int Days { get; set; }
        public double TotalDays { get; set; }
        public double TotalHours { get; set; }
        public double TotalMilliseconds { get; set; }
        public double TotalMinutes { get; set; }
        public double TotalSeconds { get; set; }
    }
}
