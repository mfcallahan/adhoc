using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace RestDump
{
    public static class Tools
    {
        //Extension methods
        public static List<List<T>> SplitList<T>(this IList<T> source, int chunkSize)
        {
            return source
                .Select((i, j) => new { Index = j, Value = i })
                .GroupBy(k => k.Index / chunkSize)
                .Select(l => l.Select(v => v.Value).ToList())
                .ToList();
        }

        public static string SanitzeFileName(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static string getDateTime()
        {
            // path to store tab file
            string yearval = DateTime.Now.Year.ToString();
            //Year(Now)
            if (int.Parse(yearval) < 10)
            {
                yearval = "0" + Convert.ToString(yearval);
            }
            string monthval = DateTime.Now.Month.ToString();
            //Month(Now)
            if (int.Parse(monthval) < 10)
            {
                monthval = "0" + Convert.ToString(monthval);
            }
            string dayval = DateTime.Now.Day.ToString();
            //Day(Now)
            if (int.Parse(dayval) < 10)
            {
                dayval = "0" + Convert.ToString(dayval);
            }
            string hourval = DateTime.Now.Hour.ToString();
            //Hour(Now)
            if (int.Parse(hourval) < 10)
            {
                hourval = "0" + Convert.ToString(hourval);
            }
            string minval = DateTime.Now.Minute.ToString();
            //Minute(Now)
            if (int.Parse(minval) < 10)
            {
                minval = "0" + Convert.ToString(minval);
            }
            string secval = DateTime.Now.Second.ToString();
            //Second(Now)
            if (int.Parse(secval) < 10)
            {
                secval = "0" + Convert.ToString(secval);
            }
            string milsecval = DateTime.Now.Millisecond.ToString();

            return yearval + "_" + monthval + "_" + dayval + "_" + hourval + minval + secval + "_" + milsecval;

        }
    }
}
