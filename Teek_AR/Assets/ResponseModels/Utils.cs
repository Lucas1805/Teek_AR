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

                    DateTime startdate = new DateTime(1970, 1, 1).ToLocalTime() + time;
                    result = startdate.ToLongDateString() +" " + startdate.ToShortTimeString();
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
        public static string JsonDateToDateTimeShortString(string JsonDateString)
        {
            string result = "";
            try
            {
                if (JsonDateString != null && JsonDateString.Length > 0)
                {
                    JsonDateString = JsonDateString.Replace("Date", "");
                    JsonDateString = JsonDateString.Replace("(", "");
                    JsonDateString = JsonDateString.Replace(")", "");
                    JsonDateString = JsonDateString.Replace("/", "");

                    TimeSpan time = TimeSpan.FromMilliseconds(double.Parse(JsonDateString));

                    DateTime startdate = new DateTime(1970, 1, 1).ToLocalTime() + time;
                    result = startdate.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    result = "Unavailable!!";
                }
            }
            catch (Exception e)
            {
                result = "Parse error!!!";
                Debug.Log("Parse Error In JsonDateToDateTimeLongString: " + e.Message);
            }
            return result;
        }

        /// <summary>
        /// This function is used to get MAC address of Wifi the phone is connected to. THIS FUNTION ONLY WORK ON ANDROID
        /// </summary>
        /// <returns>MAC Address String</returns>

        public static string getBSSID()
        {
#if UNITY_ANDROID
            string bssid = null;

            AndroidJavaObject mWiFiManager = null;
            if (mWiFiManager == null)
            {
                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    mWiFiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
                }
            }
            bssid = mWiFiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getBSSID");
            return bssid;
#endif

#if UNITY_IOS
        //NOT IMPLEMENT YET
#endif
        }

        public static List<string> TruncateLongerString(string originalString, int maxChar)
        {
            List<string> resultList = new List<string>();

            int resultListCount = originalString.Length - maxChar + 1;

            for (int i = 0; i < resultListCount; i++)
            {
                if (i == 0)
                {
                    resultList.Add(originalString.Substring(i, maxChar) + "...");
                }
                else if (i == (originalString.Length - maxChar))
                {
                    resultList.Add("..." + originalString.Substring(i, maxChar));
                }
                else
                {
                    resultList.Add("..." + originalString.Substring(i, maxChar) + "...");
                }
            }

            for (int i = 0; i < resultListCount; i++)
            {
                resultList.Add(resultList[resultListCount - i - 1]);
            }

            return resultList;
        }
    }
}
