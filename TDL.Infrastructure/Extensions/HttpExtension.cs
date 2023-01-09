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
            return context != null && context.Items.TryGetValue(CommonConstant.Id, out var userId) 
                ? (Guid)userId : Guid.Empty; 
        }

        public static string GetUserName(this HttpContext context)
        {
            return context != null && context.Items.TryGetValue(CommonConstant.UserName, out var username) 
                ? (string)username : string.Empty;
        }

        public static string GetUserRole(this HttpContext context)
        {
            return context != null && context.Items.TryGetValue(CommonConstant.UserRole, out var userrole) 
                ? (string)userrole : string.Empty;
        } 
    }
}
