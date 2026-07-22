using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Notification;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public NotificationService(IUnitOfWork unitOfWork, IRealtimeNotifier realtimeNotifier   )
        {
            _unitOfWork = unitOfWork;
            _realtimeNotifier = realtimeNotifier;
        }

        public async Task<IEnumerable<NotificationDto>> GetByUserIdAsync(int userId)
        {
            var items = await _unitOfWork.Notifications.GetByUserIdAsync(userId);
            return items.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                Title = n.Title ?? string.Empty,
                Content = n.Content ?? string.Empty,
                IsRead = n.IsRead ?? false,
                CreatedDate = n.CreatedDate ?? DateTime.MinValue
            });
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var items = await _unitOfWork.Notifications.GetByUserIdAsync(userId);
            return items.Count(n => !(n.IsRead ?? false));
        }

        public async Task CreateAsync(int userId, string title, string content)
        {
            await _unitOfWork.Notifications.AddAsync(new Notification
            {
                UserId = userId,
                Title = title,
                Content = content,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
            await _realtimeNotifier.NotifyUserAsync(userId, title, content);
        }

        public async Task MarkAsReadAsync(int notificationId, int requesterId)
        {
            var noti = await _unitOfWork.Notifications.GetByIdAsync(notificationId)
                ?? throw new AppException("Thông báo không tồn tại.");

            if (noti.UserId != requesterId)
                throw new AppException("Bạn không có quyền thao tác với thông báo này.");

            noti.IsRead = true;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var items = await _unitOfWork.Notifications.GetByUserIdAsync(userId);
            foreach (var n in items.Where(n => !(n.IsRead ?? false)))
                n.IsRead = true;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
