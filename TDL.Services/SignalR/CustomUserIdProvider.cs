using IdentityModel;
using Microsoft.AspNetCore.SignalR;
using TDL.Infrastructure.Extensions;

namespace TDL.Services.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userName = connection.User?.FindFirst(JwtClaimTypes.PreferredUserName)?.Value?.GetUserNameFromMailAddress();

            return userName;
        }
    }
}
