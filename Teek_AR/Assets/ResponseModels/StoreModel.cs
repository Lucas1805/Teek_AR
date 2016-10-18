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
        public Nullable<bool> IsAvailable { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.TimeSpan> OpenTime { get; set; }
        public Nullable<System.TimeSpan> CloseTime { get; set; }
        public Nullable<int> OrganizerId { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
