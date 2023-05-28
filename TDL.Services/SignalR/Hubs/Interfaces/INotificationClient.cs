using System.Threading.Tasks;

namespace TDL.Services.SignalR.Hubs.Interfaces
{
    public interface INotificationClient
    {
        Task Notify(object data);

        Task SendNotification(object data);

        Task PostNotify(object data);
    }
}
