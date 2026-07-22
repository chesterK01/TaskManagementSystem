using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Task;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IProjectTaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IAttachmentService _attachmentService;
        private readonly ICommentService _commentService;

        public TasksController(
            IProjectTaskService taskService,
            IProjectService projectService,
            IAttachmentService attachmentService,
            ICommentService commentService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _attachmentService = attachmentService;
            _commentService = commentService;
        }

        public async Task<IActionResult> MyTasks()
        {
            var tasks = await _taskService.GetMyTasksAsync(User.GetUserId());
            return View(tasks.Select(ToViewModel));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                var vm = ToViewModel(task);

                var isManager = User.IsInRole("Admin") || User.IsInRole("Manager");
                var isAssignee = task.AssignedUsers.Any(a => a.UserId == User.GetUserId());

                var canManage = isManager;                    // chỉ Admin/Manager: sửa, xóa, giao việc
                var canUpdateStatus = isManager || isAssignee; // + người được giao: chỉ đổi trạng thái

                if (canManage)
                {
                    var assignable = await _taskService.GetAssignableUsersAsync(id, User.GetUserId());
                    ViewBag.AssignableUsers = new SelectList(assignable, "UserAccountId", "FullName");
                }
                ViewBag.CanManage = canManage;
                ViewBag.CanUpdateStatus = canUpdateStatus;
                ViewBag.CurrentUserId = User.GetUserId();
                LoadStatusOptions();

                ViewBag.Attachments = await _attachmentService.GetByTaskIdAsync(id);
                ViewBag.Comments = await _commentService.GetByTaskIdAsync(id);
                ViewBag.History = await _taskService.GetHistoryAsync(id);

                return View(vm);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyTasks));
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(int projectId)
        {
            ViewBag.ProjectId = projectId;
            LoadPriorityOptions();
            return View(new CreateProjectTaskViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(int projectId, CreateProjectTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProjectId = projectId;
                LoadPriorityOptions();
                return View(model);
            }

            try
            {
                var dto = new CreateProjectTaskDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Priority = model.Priority,
                    DueDate = model.DueDate
                };

                var created = await _taskService.CreateAsync(projectId, dto, User.GetUserId());
                TempData["Success"] = "Đã tạo công việc mới.";
                return RedirectToAction("Details", "Projects", new { id = projectId });
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ProjectId = projectId;
                LoadPriorityOptions();
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                var model = new UpdateProjectTaskViewModel
                {
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority,
                    DueDate = task.DueDate
                };

                ViewBag.TaskId = id;
                ViewBag.ProjectId = task.ProjectId;
                LoadPriorityOptions();
                return View(model);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyTasks));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, UpdateProjectTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TaskId = id;
                LoadPriorityOptions();
                return View(model);
            }

            try
            {
                var dto = new UpdateProjectTaskDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Priority = model.Priority,
                    DueDate = model.DueDate
                };

                var updated = await _taskService.UpdateAsync(id, dto);
                TempData["Success"] = "Đã cập nhật công việc.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.TaskId = id;
                LoadPriorityOptions();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, byte status, int projectId)
        {
            try
            {
                await _taskService.UpdateStatusAsync(id, (ProjectTaskStatus)status, User.GetUserId());
                TempData["Success"] = "Đã cập nhật trạng thái công việc.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id, int projectId)
        {
            try
            {
                await _taskService.DeleteAsync(id);
                TempData["Success"] = "Đã xóa công việc.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AssignUser(int taskId, int userId)
        {
            try
            {
                await _taskService.AssignUserAsync(taskId, userId);
                TempData["Success"] = "Đã giao việc.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = taskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UnassignUser(int taskId, int userId)
        {
            try
            {
                await _taskService.UnassignUserAsync(taskId, userId);
                TempData["Success"] = "Đã hủy giao việc.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = taskId });
        }

        private void LoadPriorityOptions()
        {
            ViewBag.PriorityOptions = Enum.GetValues<TaskPriority>()
                .Select(p => new SelectListItem
                {
                    Value = ((byte)p).ToString(),
                    Text = StatusLabels.TaskPriorityName(p)
                });
        }

        private void LoadStatusOptions()
        {
            ViewBag.StatusOptions = Enum.GetValues<ProjectTaskStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((byte)s).ToString(),
                    Text = StatusLabels.TaskStatusName(s)
                });
        }

        private static ProjectTaskViewModel ToViewModel(ProjectTaskDto dto) => new()
        {
            TaskId = dto.TaskId,
            ProjectId = dto.ProjectId,
            ProjectName = dto.ProjectName,
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            PriorityName = dto.PriorityName,
            Status = dto.Status,
            StatusName = dto.StatusName,
            DueDate = dto.DueDate,
            CreatedByName = dto.CreatedByName,
            CreatedDate = dto.CreatedDate,
            AssignedUsers = dto.AssignedUsers.Select(a => new TaskAssignmentViewModel
            {
                UserId = a.UserId,
                UserName = a.UserName,
                FullName = a.FullName,
                AssignedDate = a.AssignedDate
            }).ToList()
        };
    }
}