using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class UserInfoModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ImageURL { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fullname { get; set; }
        public int Coin { get; set; }
        public int BrandCount { get; set; }
        public int EventCount { get; set; }
    }
}
