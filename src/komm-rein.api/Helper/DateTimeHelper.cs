using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public static class DateTimeHelper
    {
        public static DayOfWeek FromSystem(this System.DayOfWeek dayOfWeek)
        {
            int numeric = (int)dayOfWeek;

            return (DayOfWeek)(1 << numeric);
        }

        public static OpeningHours ForDateTime(this IEnumerable<OpeningHours> hours, DateTime dateTime)
        {
            var dayOfWeek = dateTime.DayOfWeek.FromSystem();

            var result = hours.FirstOrDefault(o =>
                (o.DayOfWeek & dayOfWeek) == dayOfWeek
                && o.From.TimeOfDay <= dateTime.TimeOfDay && o.To.TimeOfDay >= dateTime.TimeOfDay);

            return result;
        }

        public static IEnumerable<OpeningHours> ForDay(this IEnumerable<OpeningHours> hours, DateTime day)
        {
            var dayOfWeek = day.DayOfWeek.FromSystem();

            var result = hours.Where(o => (o.DayOfWeek & dayOfWeek) == dayOfWeek);

            return result;
        }

        public static IEnumerable<OpeningHours> RemainingForDay(this IEnumerable<OpeningHours> hours, DateTime day, DateTime currentTime)
        {
            var dayOfWeek = day.DayOfWeek.FromSystem();

            // important to use the date from the OpeningHours.From for comparing the time
            // using only the time part would produce the wrong result for 24h spans
            var result = hours.Where(o => (o.DayOfWeek & dayOfWeek) == dayOfWeek 
                && o.To >= (o.From.Date + currentTime.TimeOfDay));

            return result;
        }

    }
}
