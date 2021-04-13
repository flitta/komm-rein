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

        public static String Email(this ClaimsPrincipal item)
        {
            string email = item.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new SecurityException("email clain not found. Access token needs email claim");
            }

            return email;
        }
    }
}
