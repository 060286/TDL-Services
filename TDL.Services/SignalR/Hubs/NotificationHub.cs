using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TDL.Services.SignalR.Hubs.Interfaces;

namespace TDL.Services.SignalR.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {

        //public string GetWorkspaceId(HubConnectionContext connection)
        //{
        //    var workspaceId = connection.GetHttpContext().Request.Query["workspaceId"];

        //    return workspaceId;
        //}

        //private const string EbgIdQueryParam = "";

        //public override Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();

        //    //Guard.ThrowByCondition<BusinessLogicException>(!httpContext.Request.Query.ContainsKey(EbgIdQueryParam), "Not found");

        //    //var ebgId = httpContext.Request.Query["WorkspaceId"].ToString();

        //    var workspaceId = httpContext.Request.Query["workspaceId"].ToString();

        //    if(!string.IsNullOrEmpty(workspaceId))
        //    {
        //        Groups.AddToGroupAsync(Context.ConnectionId, workspaceId);
        //    }

        //    return base.OnConnectedAsync();
        //}

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



        //public async Task SendMessage(string userId, string message)
        //{
        //    if (UserConnections.TryGetValue(userId, out string connectionId))
        //    {
        //        await Clents.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        //    }
        //}
    }
}
