using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
    public static class DateTimeHelper
    {
        public static DayOfWeek FromSystem(this System.DayOfWeek dayOfWeek)
        {
            int numeric = (int)dayOfWeek;

            return (DayOfWeek)(1 << numeric);
        }

    }
}
