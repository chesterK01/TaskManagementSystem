using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;

namespace TaskManagementSystem.Web.ViewComponents
{
    public class NotificationBadgeViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public NotificationBadgeViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (UserClaimsPrincipal?.Identity?.IsAuthenticated != true)
                return Content(string.Empty);

            var count = await _notificationService.GetUnreadCountAsync(UserClaimsPrincipal.GetUserId());
            return View(count);
        }
    }
}