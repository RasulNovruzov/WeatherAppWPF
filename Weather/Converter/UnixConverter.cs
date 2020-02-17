using System;

namespace Weather.Converter
{
    static class UnixConverter
    {
        public static DateTime ConvertUnixToLocalTime(int unix)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unix).ToLocalTime();
            return dateTime;            
        }
    }
}
