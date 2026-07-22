using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Hubs;

namespace TaskManagementSystem.Web.Services
{
    public class RealtimeNotifier : IRealtimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealtimeNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUserAsync(int userId, string title, string content)
        {
            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", new { title, content });
        }
    }
}
