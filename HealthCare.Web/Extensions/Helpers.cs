using HealthCare.Web.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public static class Helpers
    {
        public static string[] ExcelCols = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };

        public static int GetWeekNumber(this DateTime date)
        {
            //Constants
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;
            bool thursdayFlag = false;

            //Get the day number since the beginning of the year
            int dayOfYear = date.DayOfYear;

            //Get the first and last weekday of the year
            int startWeekDay = (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int endWeekDay = (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            //Compensate for using monday as the first day of the week
            if (startWeekDay == 0)
            {
                startWeekDay = 7;
            }
            if (endWeekDay == 0)
            {
                endWeekDay = 7;
            }

            //Calculate the number of days in the first week
            int daysInFirstWeek = 8 - (startWeekDay);

            //Year starting and ending on a thursday will have 53 weeks
            if (startWeekDay == THURSDAY || endWeekDay == THURSDAY)
            {
                thursdayFlag = true;
            }

            //We begin by calculating the number of FULL weeks between
            //the year start and our date. The number is rounded up so
            //the smallest possible value is 0.
            int fullWeeks = (int)Math.Ceiling((dayOfYear - (daysInFirstWeek)) / 7.0);
            int result = fullWeeks;

            //If the first week of the year has at least four days, the
            //actual week number for our date can be incremented by one.
            if (daysInFirstWeek >= THURSDAY)
            {
                result = result + 1;
            }

            //If the week number is larger than 52 (and the year doesn't
            //start or end on a thursday), the correct week number is 1.
            if (result > 52 && !thursdayFlag)
            {
                result = 1;
            }

            //If the week number is still 0, it means that we are trying
            //to evaluate the week number for a week that belongs to the
            //previous year (since it has 3 days or less in this year).
            //We therefore execute this function recursively, using the
            //last day of the previous year.
            if (result == 0)
            {
                result = GetWeekNumber(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));
            }

            return result;
        }

        public static IEnumerable<DateTime> GenerateDates(DateTime fromDate, DateTime toDate)
        {
            List<DateTime> dates = new List<DateTime>();
            while (fromDate <= toDate)
            {
                dates.Add(fromDate);
                fromDate = fromDate.AddDays(1);
            }

            return dates;
        }

        public static ResponseModel Success(this ResponseModel model, string message = "Success")
        {
            model.Success = true;
            model.Message = message;
            return model;
        }

        public static ResponseModel Error(this ResponseModel model, string message = "Errors", string details = "")
        {
            model.Success = false;
            model.Message = message;
            model.Details = details;

            return model;
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }


        //public static IEnumerable<SelectListItem> EnumsToSelectList<T>(object setdValue = null, string defalutText = "", string defalutValue = "")
        //{
        //    var items = Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
        //    {
        //        Text = v.ToDescription(),
        //        Value = Convert.ToInt32(v).ToString()
        //    }).ToList();

        //    if (defalutText.IsNotNullOrEmpty())
        //    {
        //        items.Insert(0, new SelectListItem { Text = defalutText, Value = defalutValue });
        //    }

        //    if (setdValue != null)
        //    {
        //        items.SetSelected(setdValue);
        //    }

        //    return items;
        //}

        public static string ToDescription<T>(this T status)
        {
            Type enumType = typeof(T);

            MemberInfo memberInfo =
                enumType.GetMember(status.ToString()).First();
            var descriptionAttribute =
                memberInfo.GetCustomAttribute<DescriptionAttribute>();

            var description = descriptionAttribute.Description;

            return description;
        }

        //public static List<SelectListItem> SetSelected(this List<SelectListItem> items, object value)
        //{
        //    if (items != null && value != null)
        //    {
        //        var v = value.ToString();
        //        if (items.Any())
        //        {
        //            foreach (var s in items.Where(o => o.Selected == true))
        //            {
        //                s.Selected = false;
        //            }

        //            var item = items.Where(o => o.Value == v).FirstOrDefault();
        //            if (item != null)
        //            {
        //                item.Selected = true;
        //            }
        //        }
        //    }
        //    return items;
        //}


        //public static string GetConnectionString(this AppClaim appClaim)
        //{
        //    return $"Server={Constants.SqlServer};Initial Catalog={appClaim.DBName};Persist Security Info=False;User ID={Encription.Decrypt(appClaim.DBUserId)};Password={Encription.Decrypt(appClaim.DBUserPassword)};MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //}

        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }

        public static DateTime? StringToDate(this string date)
        {
            return date.IsNullOrEmpty() ? (DateTime?)null : DateTime.ParseExact(date, Constants.DateFormat, CultureInfo.InvariantCulture);
        }
        public static DateTime? StringToLastMinuteOfDate(this string date)
        {
            var dt = date.IsNullOrEmpty() ? (DateTime?)null : DateTime.ParseExact(date, Constants.DateFormat, CultureInfo.InvariantCulture);
            TimeSpan time = new TimeSpan(0, 23, 59, 59);
            return dt.Value.Add(time);
        }

        public static DateTime? StringToDateTime(this string date)
        {
            return date.IsNullOrEmpty() ? (DateTime?)null : DateTime.ParseExact(date, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
        }

        public static string FormatCurrency(this decimal value)
        {
            return value.ToString("N", new CultureInfo("sv-SE"));
        }

        public static string FormatMargin(this int value)
        {
            return $"{value} %";
        }

        public static string FormatStringArray(this string value)
        {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                List<string> valueList = value.Split(',').ToList();
                foreach (var v in valueList)
                {
                    result.Add($"'{v}'");
                }
            }
            return $"{string.Join(",", result.ToArray())}";
        }

        public static int ToInt(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? 0 : Convert.ToInt32(value);
        }

        public static int? ToNullableInt(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (int?)null : Convert.ToInt32(value);
        }


        public static int ToInt(this object value)
        {
            return value == null ? 0 : Convert.ToInt32(value);
        }

        public static string WeekDayName(this DateTime dayValue, bool isFullLength = false)
        {
            if (isFullLength)
            {
                return dayValue.ToString("dddd");
            }
            else
            {
                return dayValue.ToString("ddd");
            }
        }

        public static string ToCellValue(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return string.Empty;
            return obj;
        }

        public static bool IsEquals(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
