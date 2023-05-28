using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TDL.Services.SignalR.Hubs.Interfaces;

namespace TDL.Services.SignalR.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        //private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();

        //public override Task OnConnectedAsync()
        //{
        //    string userId = Context.UserIdentifier;
        //    string connectionId = Context.ConnectionId;

        //    if (!UserConnections.ContainsKey(userId))
        //    {
        //        UserConnections.Add(userId, connectionId);
        //    }
        //    else
        //    {
        //        UserConnections[userId] = connectionId;
        //    }

        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    string userId = Context.UserIdentifier;

        //    if (UserConnections.ContainsKey(userId))
        //    {
        //        UserConnections.Remove(userId);
        //    }

        //    return base.OnDisconnectedAsync(exception);
        //}

        //public async Task SendMessage(string userId, string message)
        //{
        //    if (UserConnections.TryGetValue(userId, out string connectionId))
        //    {
        //        await Clents.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        //    }
        //}
    }
}
