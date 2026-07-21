using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Task;
using TaskManagementSystem.Service.DTOs.TaskHistory;
using TaskManagementSystem.Service.DTOs.User;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public ProjectTaskService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetByProjectIdAsync(int projectId)
        {
            var tasks = await _unitOfWork.ProjectTasks.GetAllAsync();
            var filtered = tasks.Where(t => t.ProjectId == projectId);

            var result = new List<ProjectTaskDto>();
            foreach (var t in filtered)
                result.Add(await MapToDtoAsync(t));
            return result;
        }

        public async Task<ProjectTaskDto> GetByIdAsync(int id)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(id)
                ?? throw new AppException("Công việc không tồn tại.");
            return await MapToDtoAsync(task);
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetMyTasksAsync(int userId)
        {
            var assignments = await _unitOfWork.TaskAssignments.GetByUserIdAsync(userId);
            var result = new List<ProjectTaskDto>();
            foreach (var a in assignments)
            {
                var task = await _unitOfWork.ProjectTasks.GetByIdAsync(a.TaskId);
                if (task != null) result.Add(await MapToDtoAsync(task));
            }
            return result.OrderBy(t => t.DueDate);
        }

        public async Task<ProjectTaskDto> CreateAsync(int projectId, CreateProjectTaskDto dto, int createdBy)
        {
            _ = await _unitOfWork.Projects.GetByIdAsync(projectId)
                ?? throw new AppException("Dự án không tồn tại.");

            var task = new Repository.Models.ProjectTask
            {
                ProjectId = projectId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = (byte)ProjectTaskStatus.ToDo,
                DueDate = dto.DueDate,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.ProjectTasks.CreateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(task.TaskId);
        }

        public async Task<ProjectTaskDto> UpdateAsync(int id, UpdateProjectTaskDto dto)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(id)
                ?? throw new AppException("Công việc không tồn tại.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;

            await _unitOfWork.ProjectTasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task UpdateStatusAsync(int id, ProjectTaskStatus newStatus, int changedBy)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(id)
                ?? throw new AppException("Công việc không tồn tại.");

            var oldStatus = task.Status;
            task.Status = (byte)newStatus;
            await _unitOfWork.ProjectTasks.UpdateAsync(task);

            // Ghi lại lịch sử đổi, timeline tiến độ task
            await _unitOfWork.TaskHistories.AddAsync(new TaskHistory
            {
                TaskId = id,
                OldStatus = oldStatus,
                NewStatus = (byte)newStatus,
                ChangedBy = changedBy,
                ChangedDate = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(id)
                ?? throw new AppException("Công việc không tồn tại.");

            await _unitOfWork.ProjectTasks.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAssignableUsersAsync(int taskId)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(taskId)
                ?? throw new AppException("Công việc không tồn tại.");

            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(task.ProjectId);
            var assignments = await _unitOfWork.TaskAssignments.GetByTaskIdAsync(taskId);
            var assignedIds = assignments.Select(a => a.UserId).ToHashSet();

            return members
                .Where(m => !assignedIds.Contains(m.UserId))
                .Select(m => new UserDto
                {
                    UserAccountId = m.UserId,
                    UserName = m.User.UserName,
                    FullName = m.User.FullName,
                    Email = m.User.Email
                });
        }

        public async Task AssignUserAsync(int taskId, int userId)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(taskId)
                ?? throw new AppException("Công việc không tồn tại.");

            var isMember = (await _unitOfWork.ProjectMembers.GetByProjectIdAsync(task.ProjectId))
                .Any(m => m.UserId == userId);
            if (!isMember)
                throw new AppException("Chỉ có thể giao việc cho thành viên của dự án.");

            var alreadyAssigned = (await _unitOfWork.TaskAssignments.GetByTaskIdAsync(taskId))
                .Any(a => a.UserId == userId);
            if (alreadyAssigned)
                throw new AppException("Người dùng đã được giao công việc này.");

            await _unitOfWork.TaskAssignments.AddAsync(new TaskAssignment
            {
                TaskId = taskId,
                UserId = userId,
                AssignedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.CreateAsync(userId, "Bạn được giao công việc mới",
                $"Bạn vừa được giao công việc \"{task.Title}\".");
        }

        public async Task UnassignUserAsync(int taskId, int userId)
        {
            var assignments = await _unitOfWork.TaskAssignments.GetByTaskIdAsync(taskId);
            var assignment = assignments.FirstOrDefault(a => a.UserId == userId)
                ?? throw new AppException("Người dùng chưa được giao công việc này.");

            await _unitOfWork.TaskAssignments.RemoveAsync(assignment);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<ProjectTaskDto> MapToDtoAsync(ProjectTask task)
        {
            var assignments = await _unitOfWork.TaskAssignments.GetByTaskIdAsync(task.TaskId);

            var priority = task.Priority ?? (byte)TaskPriority.Medium;
            var status = task.Status ?? (byte)ProjectTaskStatus.ToDo;

            return new ProjectTaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                ProjectName = task.Project?.ProjectName ?? string.Empty,
                Title = task.Title,
                Description = task.Description,
                Priority = priority,
                PriorityName = StatusLabels.TaskPriorityName((TaskPriority)priority),
                Status = status,
                StatusName = StatusLabels.TaskStatusName((ProjectTaskStatus)status),
                DueDate = task.DueDate,
                CreatedBy = task.CreatedBy,
                CreatedByName = task.CreatedByNavigation?.FullName ?? string.Empty,
                CreatedDate = task.CreatedDate ?? DateTime.MinValue,
                AssignedUsers = assignments.Select(a => new TaskAssignmentDto
                {
                    TaskAssignmentId = a.TaskAssignmentId,
                    UserId = a.UserId,
                    UserName = a.User.UserName,
                    FullName = a.User.FullName,
                    AssignedDate = a.AssignedDate ?? DateTime.MinValue
                }).ToList()
            };
        }
        public async Task<IEnumerable<TaskHistoryDto>> GetHistoryAsync(int taskId)
        {
            var histories = await _unitOfWork.TaskHistories.GetByTaskIdAsync(taskId);
            return histories.Select(h => new TaskHistoryDto
            {
                OldStatus = h.OldStatus ?? 0,
                OldStatusName = StatusLabels.TaskStatusName((ProjectTaskStatus)(h.OldStatus ?? 0)),
                NewStatus = h.NewStatus ?? 0,
                NewStatusName = StatusLabels.TaskStatusName((ProjectTaskStatus)(h.NewStatus ?? 0)),
                ChangedByName = h.ChangedByNavigation?.FullName ?? string.Empty,
                ChangedDate = h.ChangedDate ?? DateTime.MinValue
            });
        }
    }
}
