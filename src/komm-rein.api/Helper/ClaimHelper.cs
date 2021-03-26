using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimHelper
    {
        public static String Sid(this ClaimsPrincipal item)
        {
            return item.FindFirstValue("sid");
        }
    }
}
