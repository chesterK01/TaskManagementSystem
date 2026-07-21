using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class AttachmentsController : Controller
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IWebHostEnvironment _env;
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;

        public AttachmentsController(IAttachmentService attachmentService, IWebHostEnvironment env)
        {
            _attachmentService = attachmentService;
            _env = env;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn file.";
                return RedirectToAction("Details", "Tasks", new { id = taskId });
            }

            if (file.Length > MaxFileSizeBytes)
            {
                TempData["Error"] = "File không được vượt quá 10MB.";
                return RedirectToAction("Details", "Tasks", new { id = taskId });
            }

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "tasks", taskId.ToString());
                Directory.CreateDirectory(uploadsFolder);

                // Đặt tên file  bằng guid để tránh trùng lặp, ký tự lạ, vẫn giữ file name gốc riêng để hiển thị cho user
                var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var physicalPath = Path.Combine(uploadsFolder, safeFileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/uploads/tasks/{taskId}/{safeFileName}";
                await _attachmentService.AddAsync(taskId, file.FileName, relativePath, User.GetUserId());

                TempData["Success"] = "Đã tải file lên.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "Tasks", new { id = taskId });
        }

        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var (filePath, fileName) = await _attachmentService.GetFileForDownloadAsync(id);
                var physicalPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (!System.IO.File.Exists(physicalPath))
                {
                    TempData["Error"] = "File không tồn tại trên hệ thống.";
                    return RedirectToAction("Index", "Home");
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
                return File(bytes, "application/octet-stream", fileName);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int attachmentId, int taskId)
        {
            try
            {
                var filePath = await _attachmentService.DeleteAsync(attachmentId, User.GetUserId(), User.IsInRole("Admin"));

                var physicalPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(physicalPath))
                    System.IO.File.Delete(physicalPath);

                TempData["Success"] = "Đã xóa file.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", "Tasks", new { id = taskId });
        }
    }
}