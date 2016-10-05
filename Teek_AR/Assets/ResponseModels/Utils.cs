//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Assets.ResponseModels
//{
//    class Utils
//    {
//        public DateTimeOffset GetDateTimeOffset(string inDate)
//        {
//            string delimiter = "-";

//            if (inDate.IndexOf("+") != -1)
//            {
//                delimiter = "+";
//            }

//            string[] dateParts = inDate.Split(new string[] { delimiter }, 2, System.StringSplitOptions.None);

//            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

//            var parsedDate = epoch.AddMilliseconds(Convert.ToDouble(dateParts[0]));

//            var offset = TimeSpan.ParseExact(dateParts[1], "hhmm", null, delimiter == "-" ? System.Globalization.TimeSpanStyles.AssumeNegative : System.Globalization.TimeSpanStyles.None);

//            return new DateTimeOffset(parsedDate, offset);
//        }
//    }
//}
