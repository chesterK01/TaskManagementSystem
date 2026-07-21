using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _notificationService.GetByUserIdAsync(User.GetUserId());
            var viewModels = items.Select(n => new NotificationViewModel
            {
                NotificationId = n.NotificationId,
                Title = n.Title,
                Content = n.Content,
                IsRead = n.IsRead,
                CreatedDate = n.CreatedDate
            }).OrderByDescending(n => n.CreatedDate);

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id, User.GetUserId());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await _notificationService.MarkAllAsReadAsync(User.GetUserId());
            return RedirectToAction(nameof(Index));
        }
    }
}
