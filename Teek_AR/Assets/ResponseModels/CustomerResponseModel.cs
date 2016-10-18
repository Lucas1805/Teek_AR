using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class CustomerResponseModel
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public string UserId { get; set; }
        public System.DateTime JoinDate { get; set; }
        public bool Active { get; set; }
        public int Coin { get; set; }
        public int Teek { get; set; }
        public int Ruby { get; set; }
        public int Sapphire { get; set; }
        public int Citrine { get; set; }
        public int Fireball { get; set; }
        public int Iceball { get; set; }

    }
}
