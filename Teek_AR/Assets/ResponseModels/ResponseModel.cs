using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class ResponseModel<T> where T:class
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public T Data { get; set; }

    }
}
