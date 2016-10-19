using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public static class Utils
    {
        public static string TruncateLongString(string OriginalString, int MaxChar)
        {
            string result = OriginalString;

            if (OriginalString.Length > MaxChar - 3)
            {
                result = OriginalString.Substring(0, MaxChar - 3);
                result = result + "...";
            }

            return result;
        }

    }
}
