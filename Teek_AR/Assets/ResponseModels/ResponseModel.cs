using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public class ResponseModel<T> where T:class
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

    }
    public class RatingResponseModel
    {
        public int RateCount { get; set; }
        public double Point { get; set; }
        public double UserRate { get; set; }
    }
}
