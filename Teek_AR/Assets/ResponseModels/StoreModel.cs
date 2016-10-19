using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public class StoreModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsAvailable { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string CreateDate { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public int OrganizerId { get; set; }
        public bool Active { get; set; }
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
