using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class PrizeCodeModel
    {
        public  int Id { get; set; }
        public  int PrizeId { get; set; }
        public  int UserParticipationId { get; set; }
        public  bool Status { get; set; }
        public  DateTime Date { get; set; }
    }
}
