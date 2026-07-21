using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Task;
using TaskManagementSystem.Service.DTOs.TaskHistory;
using TaskManagementSystem.Service.DTOs.User;

namespace TaskManagementSystem.Service.IServices
{
    public interface IProjectTaskService
    {
        Task<IEnumerable<ProjectTaskDto>> GetByProjectIdAsync(int projectId);
        Task<ProjectTaskDto> GetByIdAsync(int id);
        Task<IEnumerable<ProjectTaskDto>> GetMyTasksAsync(int userId);

        Task<ProjectTaskDto> CreateAsync(int projectId, CreateProjectTaskDto dto, int createdBy);
        Task<ProjectTaskDto> UpdateAsync(int id, UpdateProjectTaskDto dto);
        Task UpdateStatusAsync(int id, ProjectTaskStatus newStatus, int changedBy);
        Task DeleteAsync(int id);
        Task<IEnumerable<TaskHistoryDto>> GetHistoryAsync(int taskId);
        Task<IEnumerable<UserDto>> GetAssignableUsersAsync(int taskId);
        Task AssignUserAsync(int taskId, int userId);
        Task UnassignUserAsync(int taskId, int userId);
    }
}
