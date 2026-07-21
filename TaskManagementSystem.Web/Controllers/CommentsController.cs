using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int taskId, string content)
        {
            try
            {
                await _commentService.AddAsync(taskId, User.GetUserId(), content);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", "Tasks", new { id = taskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int commentId, int taskId)
        {
            try
            {
                await _commentService.DeleteAsync(commentId, User.GetUserId(), User.IsInRole("Admin"));
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", "Tasks", new { id = taskId });
        }
    }
}
