using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeFormatHelper
    {
        public static string ToInvariantString(this DateTime dateTime)
        {
            return dateTime.ToString("s", CultureInfo.InvariantCulture);
        }
    }
}
