using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class CouponModel
    {
        public virtual string Code { get; set; }
        public virtual int Teek { get; set; }
        public virtual string CreateDate { get; set; }
        public virtual string RedeemDate { get; set; }
        public virtual bool Active { get; set; }
        public virtual int OrganizerId { get; set; }
        public virtual string UserId { get; set; }
        public int? NewTeek { get; set; }
    }
}
