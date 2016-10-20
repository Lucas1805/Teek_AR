using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class CustomerResponseModel
    {
        public virtual int Id { get; set; }
        public virtual int OrganizerId { get; set; }
        public virtual string UserId { get; set; }
        public virtual string JoinDate { get; set; }
        public virtual bool Active { get; set; }
        public int? Coin { get; set; }
        public virtual Nullable<int> Teek { get; set; }
        public virtual Nullable<int> Ruby { get; set; }
        public virtual Nullable<int> Sapphire { get; set; }
        public virtual Nullable<int> Citrine { get; set; }
        public virtual Nullable<int> Fireball { get; set; }
        public virtual Nullable<int> Iceball { get; set; }

    }
}
