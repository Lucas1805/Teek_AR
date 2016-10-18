using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public class OrganizerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }

        public int StoreCount { get; set; }
        public bool HasARGAME { get; set; }
        public bool HasMultiplier { get; set; }
        public bool HasSurvey { get; set; }
        public bool HasVoting { get; set; }
    }
}
