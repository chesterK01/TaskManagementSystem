using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagementSystem.Web.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // chỉ dùng để sv đẩy dữ liệu xuống
    }
}
