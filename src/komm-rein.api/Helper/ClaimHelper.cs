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
            string nameidentifier = item.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if(string.IsNullOrWhiteSpace(nameidentifier))
            {
                throw new SecurityException("nameidentifier clain not found. User is not authorized!");
            }

            return nameidentifier;
        }
    }
}
