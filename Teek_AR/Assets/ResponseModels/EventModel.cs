using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class EventModel
    {
        public int Id { get; set; }
        public  string CreatorUserID { get; set; }
        public  int OrganizerId { get; set; }
        public  string Name { get; set; }
        public  string ImageUrl { get; set; }
        public  string Description { get; set; }
        public  string StartDate { get; set; }
        public  string EndDate { get; set; }
        public  string Address { get; set; }
        public  double Latitude { get; set; }
        public  double Longitude { get; set; }
        public  string CreatedTime { get; set; }
        public  string SeoName { get; set; }
        public  bool IsFree { get; set; }
        public  double Price { get; set; }
        public  int Status { get; set; }
        public  bool EnableInteractionPage { get; set; }
        public  bool Active { get; set; }
        public string MasterCode { get; set; }
        public List<ActivityModel> Activities { get; set; }
        public List<string> Categories { get; set; }
    }
}
