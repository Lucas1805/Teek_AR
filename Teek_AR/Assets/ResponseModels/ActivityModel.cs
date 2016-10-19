using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public partial class ActivityModel
    {
        public virtual int Id { get; set; }
        public virtual Nullable<int> SurveyId { get; set; }
        public virtual Nullable<int> VotingId { get; set; }
        public virtual Nullable<int> GameId { get; set; }
        public virtual string ActivityKey { get; set; }
        public virtual int EventId { get; set; }
        public virtual int Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndTime { get; set; }
        public virtual bool IsPublished { get; set; }
        public virtual bool Active { get; set; }
    }
}
