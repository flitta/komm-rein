using System;

namespace komm_rein.api.Models
{
    [Flags]
    public enum DayOfWeek
    {
        None = 0,
        Sunday = 1 << 0,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6,
        Weekend = 1 << 0 | 1 << 6,
        WorkDays = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5,
        All = short.MaxValue
    }


    public class OpeningHours
    {
       
      
        public Guid ID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        //public static FlagDaysOfWeek AllWeekdays
        //{
        //    get =>
        //        DayOfWeek.Sunday
        //        | DayOfWeek.Monday
        //        | DayOfWeek.Tuesday
        //        | DayOfWeek.Wednesday
        //        | DayOfWeek.Thursday
        //        | DayOfWeek.Friday
        //        | DayOfWeek.Sunday
        //        | DayOfWeek.Saturday;
        //}

        //public static FlagDaysOfWeek AllBussinnesDays
        //{
        //    get =>
        //        DayOfWeek.Monday
        //        | DayOfWeek.Tuesday
        //        | DayOfWeek.Wednesday
        //        | DayOfWeek.Thursday
        //        | DayOfWeek.Friday;
        //}

        //public static FlagDaysOfWeek Weekend
        //{
        //    get =>
        //        DayOfWeek.Sunday
        //        | DayOfWeek.Saturday;
        //}

    }
}