using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Project;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectTaskService _taskService;
        public ProjectsController(IProjectService projectService, IProjectTaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            return View(projects.Select(ToViewModel));
        }

        public async Task<IActionResult> MyProjects()
        {
            var projects = await _projectService.GetMyProjectsAsync(User.GetUserId());
            return View(projects.Select(ToViewModel));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                var members = await _projectService.GetMembersAsync(id);
                var tasks = await _taskService.GetByProjectIdAsync(id);

                var isManager = User.IsInRole("Admin") || User.IsInRole("Manager");
                var isOwner = project.CreatedBy == User.GetUserId();

                var vm = new ProjectDetailsViewModel
                {
                    Project = ToViewModel(project),
                    Members = members.Select(m => new ProjectMemberViewModel
                    {
                        UserId = m.UserId,
                        UserName = m.UserName,
                        FullName = m.FullName,
                        JoinedDate = m.JoinedDate
                    }).ToList(),
                    Tasks = tasks.Select(t => new ProjectTaskViewModel
                    {
                        TaskId = t.TaskId,
                        ProjectId = t.ProjectId,
                        ProjectName = t.ProjectName,
                        Title = t.Title,
                        Description = t.Description,
                        Priority = t.Priority,
                        PriorityName = t.PriorityName,
                        Status = t.Status,
                        StatusName = t.StatusName,
                        DueDate = t.DueDate,
                        CreatedByName = t.CreatedByName,
                        CreatedDate = t.CreatedDate,
                        AssignedUsers = t.AssignedUsers.Select(a => new TaskAssignmentViewModel
                        {
                            UserId = a.UserId,
                            UserName = a.UserName,
                            FullName = a.FullName,
                            AssignedDate = a.AssignedDate
                        }).ToList()
                    }).ToList(),
                    CanManage = isManager || isOwner
                };

                if (vm.CanManage)
                {
                    var availableUsers = await _projectService.GetAvailableUsersAsync(id);
                    ViewBag.AvailableUsers = new SelectList(availableUsers, "UserAccountId", "FullName");
                }

                return View(vm);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyProjects));
            }
        }

        public IActionResult Create() => View(new CreateProjectViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var dto = new CreateProjectDto
                {
                    ProjectName = model.ProjectName,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

                var created = await _projectService.CreateAsync(dto, User.GetUserId());
                TempData["Success"] = "Đã tạo dự án mới.";
                return RedirectToAction(nameof(Details), new { id = created.ProjectId });
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                var model = new UpdateProjectViewModel
                {
                    ProjectName = project.ProjectName,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status
                };

                ViewBag.ProjectId = id;
                LoadStatusOptions();
                return View(model);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, UpdateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProjectId = id;
                LoadStatusOptions();
                return View(model);
            }

            try
            {
                var dto = new UpdateProjectDto
                {
                    ProjectName = model.ProjectName,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = model.Status
                };

                await _projectService.UpdateAsync(id, dto);
                TempData["Success"] = "Đã cập nhật dự án.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ProjectId = id;
                LoadStatusOptions();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectService.DeleteAsync(id);
                TempData["Success"] = "Đã xóa dự án.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int projectId, int userId)
        {
            try
            {
                await _projectService.AddMemberAsync(projectId, userId);
                TempData["Success"] = "Đã thêm thành viên vào dự án.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int projectId, int userId)
        {
            try
            {
                await _projectService.RemoveMemberAsync(projectId, userId);
                TempData["Success"] = "Đã xóa thành viên khỏi dự án.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        private void LoadStatusOptions()
        {
            ViewBag.StatusOptions = Enum.GetValues<ProjectStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((byte)s).ToString(),
                    Text = StatusLabels.ProjectStatusName(s)
                });
        }

        private static ProjectViewModel ToViewModel(ProjectDto dto) => new()
        {
            ProjectId = dto.ProjectId,
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            StatusName = dto.StatusName,
            CreatedByName = dto.CreatedByName,
            CreatedDate = dto.CreatedDate,
            MemberCount = dto.MemberCount,
            TaskCount = dto.TaskCount
        };
    }
}