using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class DropRateModel
    {
        public virtual int Id { get; set; }
        public virtual decimal DropRateCombo1 { get; set; }
        public virtual decimal DropRateCombo2 { get; set; }
        public virtual decimal DropRateCombo3 { get; set; }
        public virtual int GameId { get; set; }
        public virtual bool Active { get; set; }
    }
}
