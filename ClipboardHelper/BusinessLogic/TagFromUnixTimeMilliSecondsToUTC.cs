using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagFromUnixTimeMilliSecondsToUtc
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private static readonly double MaxUnixSeconds = (DateTime.MaxValue - UnixEpoch).TotalSeconds;

        public static string Handle(List<string> splitContent)
        {
            if (splitContent == null) return null;
            var unixTimestamp = splitContent[0];
            if (splitContent.Count != 2) return "Couldn't parse Unix timestamp.";
            if (string.IsNullOrEmpty(unixTimestamp))
                return "Couldn't parse Unix timestamp, it's empty";
            // ReSharper disable once RedundantIfElseBlock (reSharper displays wrong)
            else
                try
                {
                    var timeStamp = UnixTimeStampToDateTime(Convert.ToDouble(unixTimestamp, CultureInfo.InvariantCulture));
                    timeStamp = timeStamp.AddHours(double.Parse(splitContent[1], CultureInfo.InvariantCulture));
                    return timeStamp.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return "[]";
                    //throw;
                }
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            return unixTimeStamp > MaxUnixSeconds
                ? UnixEpoch.AddMilliseconds(unixTimeStamp)
                : UnixEpoch.AddSeconds(unixTimeStamp);
        }
    }
}