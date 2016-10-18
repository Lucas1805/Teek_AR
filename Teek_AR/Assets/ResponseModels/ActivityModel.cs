using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class ActivityModel
    {

        public  int Id { get; set; }
        public  int SurveyId { get; set; }
        public  int VotingId { get; set; }
        public  int GameId { get; set; }
        public  string ActivityKey { get; set; }
        public  int EventId { get; set; }
        public  int Type { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }
        public  DateTime StartTime { get; set; }
        public  DateTime EndTime { get; set; }
        public  bool IsPublished { get; set; }
        public  bool Active { get; set; }
    }
}
