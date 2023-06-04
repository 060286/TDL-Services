using Microsoft.AspNetCore.SignalR;

namespace TDL.Services.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userName = connection.GetHttpContext().Request.Query["userName"];

            return userName;
        }
    }
}
