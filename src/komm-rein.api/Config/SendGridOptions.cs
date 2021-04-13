using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Config
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string SendGridUser { get; set; }
        public string From { get; set; }
        public bool EnableClickTracking { get;  set; }
        public bool EnableClickTrackingText { get; set; }
    }
}
