using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Project;
using TaskManagementSystem.Service.DTOs.User;

namespace TaskManagementSystem.Service.IServices
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<IEnumerable<ProjectDto>> GetMyProjectsAsync(int userId);
        Task<ProjectDto> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto, int createdBy);
        Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto, int createdBy, bool creatorIsAdmin);
        Task<IEnumerable<ProjectMemberDto>> GetMembersAsync(int projectId);
        Task<IEnumerable<UserDto>> GetAvailableUsersAsync(int projectId);
        Task AddMemberAsync(int projectId, int userId);
        Task RemoveMemberAsync(int projectId, int userId);
        Task<bool> IsMemberAsync(int projectId, int userId);
    }
}
