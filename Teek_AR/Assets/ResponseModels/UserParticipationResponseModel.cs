using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class UserParticipationResponseModel
    {
            public  int Id { get; set; }
            public  string UserId { get; set; }
            public  int EventId { get; set; }
            public  Nullable<int> TicketId { get; set; }
            public  string ParticipationTime { get; set; }
            public  string Token { get; set; }
            public  string ParticipateCode { get; set; }
            public  string QRCodeUrl { get; set; }
            public  bool CheckedIn { get; set; }
            public  bool Active { get; set; }
            
    }
}
