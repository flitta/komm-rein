using System;

namespace komm_rein.model
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
        All = Weekend | WorkDays
    }

    public class OpeningHours : ContextItem
    {
        public DayOfWeek DayOfWeek { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}