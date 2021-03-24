using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.oidc.Services
{
    public class IdentityServer4Options
    {
        public const string SECTION = "IdentityServer4";

        public string ClientUrl { get; set; }
    }
}
