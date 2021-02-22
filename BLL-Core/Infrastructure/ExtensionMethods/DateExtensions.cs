using System;
using System.Linq;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class DateExtensions
    {
        public static string FullDateString(this DateTime date)
        {
            return date.ToString("dddd MMMM dd, yyyy");
        }

        public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate, bool includeTime = false)
        {
            return date.IsAfter(startDate, includeTime) && date.IsBefore(endDate, includeTime);
        }

        /// <summary>
        /// Determines if a date is before a specified date. Ignores time.
        /// </summary>
        /// <param name="date">The base date</param>
        /// <param name="laterDate">The later date to compare against</param>
        /// <param name="includeTime">Optional boolean to indicate whether to include the time in the comparison - default = false</param>
        /// <returns>True if the date is before than the later date or if the later date is DateTime.MaxValue</returns>
        public static bool IsBefore(this DateTime date, DateTime laterDate, bool includeTime = false)
        {
            return includeTime
                       ? laterDate == DateTime.MaxValue || date <= laterDate
                       : laterDate.Date == DateTime.MaxValue.Date || date.Date <= laterDate.Date;
        }

        ///  <summary>
        /// Determines if a date is after a specified date
        /// </summary>
        /// <param name="date">The base date</param>
        /// <param name="priorDate">The previous date to compare against</param>
        /// <param name="includeTime">Optional boolean to indicate whether to include the time in the comparison - default = false</param>
        /// <returns>True if the date is later than the prior date or if the prior date is DateTime.MinValue</returns>
        public static bool IsAfter(this DateTime date, DateTime priorDate, bool includeTime = false)
        {
            return includeTime 
                       ? priorDate == DateTime.MinValue || date >= priorDate
                       : priorDate.Date == DateTime.MinValue.Date || date.Date >= priorDate.Date;
        }

        public static DateTime FirstDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDateOfMonth(this DateTime date)
        {
            return FirstDateOfMonth(date).AddMonths(1).AddMilliseconds(-1);
        }

        public static double GetTimeStamp(DateTime date)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return (long)(date - sTime).TotalSeconds;
        }
        /// <summary>
        /// DateWithDaySuffix
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="format">Format with string like suffix position with 0(dd{0} MM YYY)</param>
        /// <returns></returns>
        public static string DateWithDaySuffix(DateTime date, string format)
        {
            string formattedStr = string.Empty;
            if (date != DateTime.MinValue)
            {
                var daysuffixtext = "";
                if (new[] { 11, 12, 13 }.Contains(date.Day))
                {
                    daysuffixtext = "th";
                }
                else if (date.Day % 10 == 1)
                {
                    daysuffixtext = "st";
                }
                else if (date.Day % 10 == 2)
                {
                    daysuffixtext = "nd";
                }
                else if (date.Day % 10 == 3)
                {
                    daysuffixtext = "rd";
                }
                else
                {
                    daysuffixtext = "th";
                }
                formattedStr = string.Format(date.ToString(format), daysuffixtext);
            }
            return formattedStr;
        }

        public static int CalculateAge(DateTime dob)
        {
            var age = 0;
            var today = DateTime.Today;
            if (today.Year > dob.Year)
            {
                age = today.Year - dob.Year;
                if (today.DayOfYear < dob.DayOfYear) age -=1;
            }
            return age;
        }

        //To get datetime for SmartGroup Time field (hh:mm:ss:ff) Hours:Minutes:Seconds:MilliSeconds
        public static DateTime GetDateTimeForHHMMSSFF(string timeString_HHMMSSFF)
        {
            DateTime dateTime = new DateTime();
            if (timeString_HHMMSSFF.IsNotNullOrEmpty())
            {
                try
                {
                    var timeArray = timeString_HHMMSSFF.Split(':');
                    if (timeArray.IsNotNullOrEmpty())
                    {
                        if (timeArray.Count() == 4)
                        {
                            timeArray[3] = timeArray[3] + "0";
                            dateTime = new DateTime().AddHours(Convert.ToDouble(timeArray[0])).AddMinutes(Convert.ToDouble(timeArray[1])).AddSeconds(Convert.ToDouble(timeArray[2])).AddMilliseconds(Convert.ToDouble(timeArray[3]));
                        }
                        else if (timeArray.Count() == 3)
                        {
                            dateTime = new DateTime().AddHours(Convert.ToDouble(timeArray[0])).AddMinutes(Convert.ToDouble(timeArray[1])).AddSeconds(Convert.ToDouble(timeArray[2]));
                        }
                        else if (timeArray.Count() == 2)
                        {
                            dateTime = new DateTime().AddHours(Convert.ToDouble(timeArray[0])).AddMinutes(Convert.ToDouble(timeArray[1]));
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    dateTime = new DateTime();
                }
            }
            return dateTime;
        }
		
		//To set proper time for Time field (hh:mm:ss:ff) Hours:Minutes:Seconds:MilliSeconds
        public static string GetProperTimeStringForHHMMSSFF(string timeString_HHMMSSFF)
        {
            string timeString = "00:00:00:00";
            if (timeString_HHMMSSFF.IsNotNullOrEmpty())
            {
                string tempTime = "";
                try
                {
                    var timeArray = timeString_HHMMSSFF.Split(':');
                    if (timeArray.IsNotNullOrEmpty())
                    {
                        if(timeArray.Count()==4)
                        {
                            tempTime = ((timeArray[0].IsNotNullOrEmpty() && !timeArray[0].ToLower().Contains("h")) ? timeArray[0] : "00") + ":";
                            tempTime += ((timeArray[1].IsNotNullOrEmpty() && !timeArray[1].ToLower().Contains("m")) ? timeArray[1] : "00") + ":";
                            tempTime += ((timeArray[2].IsNotNullOrEmpty() && !timeArray[2].ToLower().Contains("s")) ? timeArray[2] : "00") + ":";
                            tempTime += ((timeArray[3].IsNotNullOrEmpty() && !(timeArray[3].ToLower().Contains("m") || timeArray[3].ToLower().Contains("s"))) ? timeArray[3] : "00");
                        }
                    }

                    timeString = tempTime.IsNotNullOrEmpty() ? tempTime : timeString;
                }
                catch (System.Exception ex) { }
            }
            return timeString;
        }

        //to get date in Today, Tomorrow format
        public static string GetDateInTodayTomorrowFormat(DateTime dateTime, TimeSpan? time)
        {
            var dateString = "";
            var timeString = "";
            if (dateTime.IsNotNull() && dateTime != DateTime.MinValue)
            {
                if (dateTime.Date == DateTime.Now.Date)
                    dateString += "Today";
                else if (dateTime.Date == DateTime.Now.Date.AddDays(1).Date)
                    dateString += "Tomorrow";
                else
                    dateString += dateTime.ToString("dd MMM yyyy");

                if (time.IsNull())
                    timeString = dateTime.ToShortTimeString() != "00:00" ? dateTime.ToShortTimeString() : "";
                else
                    timeString = time.Value > TimeSpan.Zero ? time.Value.ToString(@"hh\:mm") : "";

                dateString += timeString.IsNotNullOrEmpty() ? ", " + timeString : "";
            }
            return dateString;
        }

        public static DateTime ConvertFromUnixTimestamp(double seconds)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(seconds);
        }
    }
}
