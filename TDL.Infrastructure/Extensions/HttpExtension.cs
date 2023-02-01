using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDL.Infrastructure.Constants;

namespace TDL.Infrastructure.Extensions
{
    public static class HttpExtension
    {
        public static string GetTimeZone(this HttpContext context)
        {
            if(context == null)
            {
                return string.Empty;
            }

            return context.Request.Headers[ConfigurationConstant.TimeZoneKey].FirstOrDefault();
        }

        public static string GetBaseUrl(this HttpContext context)
        {
            if(context == null)
            {
                return string.Empty;
            }

            var protocal = context.Request.IsHttps ? "https" : "http";

            return $"{protocal}://{context.Request.Host}";
        }

        public static Guid GetUserId(this HttpContext context)
        {
            string userIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var userId = context.User.Claims.FirstOrDefault(x => x.Type.Equals(userIdClaimType))?.Value;
            return !string.IsNullOrEmpty(userId)
                ? new Guid(userId) : Guid.Empty;
        }

        public static string GetUserName(this HttpContext context)
        {
            string userNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

            var username = context.User.Claims.FirstOrDefault(x => x.Type.Equals(userNameClaimType))?.Value;

            return !string.IsNullOrEmpty(username)
                ? username : string.Empty;
        }

        public static string GetUserRole(this HttpContext context)
        {
            string userEmailClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

            var userEmail = context.User.Claims.FirstOrDefault(x => x.Type.Equals(userEmailClaimType))?.Value;

            return !string.IsNullOrEmpty(userEmail) 
                ? userEmail : string.Empty;
        } 
    }
}
