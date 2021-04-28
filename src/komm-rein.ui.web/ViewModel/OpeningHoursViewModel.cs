using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.ViewModel
{
    public class OpeningHoursViewModel
    {
        public static  OpeningHoursViewModel Create()
        {
            return new()
            {
                From = "",
                To = "",
                Sunday = false,
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
            };
        }

        public string From { get; set; }
        
        public string To { get; set; }

        public bool Sunday { get; set; }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday{ get; set; }

    }
}
