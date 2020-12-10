using System;

namespace Core.Helpers
{
    public static class DateHelper
    {
        public class Date
        {
            public string From { get; set; }
            public string To { get; set; }
        }
        public static Date DateFormat(string from, string to)
        {
            var fromDate = default(DateTime);
            var toDate = default(DateTime);
            if (!string.IsNullOrEmpty(from))
            {
                fromDate = Convert.ToDateTime(from);
            }

            if (!string.IsNullOrEmpty(to))
            {
                toDate = Convert.ToDateTime(to);
            }

            var res = new Date
            {
                From = fromDate.ToShortDateString(),
                To = toDate.ToShortDateString()
            };
            return res;
        }
        /// <summary>
        /// 当前时间秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 时间转UTC秒
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampTotalseconds(DateTime dateTime)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 0, 0, 0);
            long timeStamp = Convert.ToInt64((dateTime - dateStart).TotalSeconds);
            return timeStamp;
        }
        /// <summary>
        /// 毫秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStampTotalMilliseconds(DateTime dt)
        {

            DateTime dateStart = new DateTime(1970, 1, 1, 0, 0, 0);
            long timeStamp = Convert.ToInt64((dt - dateStart).TotalMilliseconds);
            return timeStamp;
        }
        /// <summary>
        /// 时间戳转日期（秒）
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime UnixSecondsTimestampToDateTime(long unix)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            DateTime newTime = dtStart.AddSeconds(unix);
            return newTime;

        }
        /// <summary>
        /// 时间戳转日期（毫秒）
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime UnixMillisecondsTimestampToDateTime(long unix)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            DateTime newTime = dtStart.AddMilliseconds(unix);
            return newTime;

        }

        /// <summary>
        /// 根据身份证号获取生日
        /// </summary>
        /// <param name="IdCard">身份证号</param>
        /// <returns></returns>
        public static DateTime BrithdayFromIdCard(string IdCard)
        {
            try
            {
                string rtn = "1900-01-01"; if (IdCard.Length == 15)
                {
                    rtn = IdCard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                }
                else if (IdCard.Length == 18)
                {
                    rtn = IdCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                }
                return Convert.ToDateTime(rtn);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberDate"></param>
        /// <returns></returns>
        public static DateTime NumberDateStringToDatetime(string numberDate)
        {
            try
            {
                var datetimeString = numberDate.Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-");
                return Convert.ToDateTime(datetimeString);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
