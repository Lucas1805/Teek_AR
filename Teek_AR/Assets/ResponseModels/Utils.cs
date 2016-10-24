using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.ResponseModels
{
    public static class Utils
    {
        public static string TruncateLongString(string OriginalString, int MaxChar)
        {
            string result = OriginalString;

            if (OriginalString.Length > MaxChar - 3 && OriginalString != null && OriginalString.Length > 0)
            {
                result = OriginalString.Substring(0, MaxChar - 3);
                result = result + "...";
            }

            return result;
        }

        public static string JsonDateToDateTimeLongString(string JsonDateString)
        {
            string result = "";
            try
            {
                if(JsonDateString != null && JsonDateString.Length > 0)
                {
                    JsonDateString = JsonDateString.Replace("Date", "");
                    JsonDateString = JsonDateString.Replace("(", "");
                    JsonDateString = JsonDateString.Replace(")", "");
                    JsonDateString = JsonDateString.Replace("/", "");

                    TimeSpan time = TimeSpan.FromMilliseconds(double.Parse(JsonDateString));

                    DateTime startdate = new DateTime(1970, 1, 1) + time;
                    
                    result = startdate.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    result = "Unavailable!!";
                }
            }
            catch(Exception e)
            {
                result = "Parse error!!!";
                Debug.Log("Parse Error In JsonDateToDateTimeLongString: " + e.Message);
            }
            return result;
        }

    }
}
