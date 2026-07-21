using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Notification;

namespace TaskManagementSystem.Service.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task CreateAsync(int userId, string title, string content);
        Task MarkAsReadAsync(int notificationId, int requesterId);
        Task MarkAllAsReadAsync(int userId);
    }
}
