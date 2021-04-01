using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Config
{
    public class ApiConfig
    {
        public const string API_NAME = "komm-rein.api";
        public const string PUBLIC_API_NAME = "komm-rein.public";

        public string Path { get; set; }

        public string ApiName { get; set; }
    }
}
