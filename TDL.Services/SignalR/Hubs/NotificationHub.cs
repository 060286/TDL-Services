using Microsoft.AspNetCore.SignalR;
using TDL.Services.SignalR.Hubs.Interfaces;

namespace TDL.Services.SignalR.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}
