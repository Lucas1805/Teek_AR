using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class PrizeResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int EventId { get; set; }
        public bool Active { get; set; }
        public int Teek { get; set; }
        public int Ruby { get; set; }
        public int Sapphire { get; set; }
        public int Citrine { get; set; }
    }
}
