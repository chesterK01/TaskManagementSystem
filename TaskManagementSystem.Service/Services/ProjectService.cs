using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.Project;
using TaskManagementSystem.Service.DTOs.User;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            var result = new List<ProjectDto>();
            foreach (var p in projects)
                result.Add(await MapToDtoAsync(p));
            return result;
        }

        public async Task<IEnumerable<ProjectDto>> GetMyProjectsAsync(int userId)
        {
            var memberships = await _unitOfWork.ProjectMembers.GetByUserIdAsync(userId);
            var result = new List<ProjectDto>();
            foreach (var m in memberships)
            {
                var project = await _unitOfWork.Projects.GetByIdAsync(m.ProjectId);
                if (project != null) result.Add(await MapToDtoAsync(project));
            }
            return result;
        }

        public async Task<ProjectDto> GetByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id)
                ?? throw new AppException("Dự án không tồn tại.");
            return await MapToDtoAsync(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int createdBy)
        {
            if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                throw new AppException("Ngày kết thúc không được trước ngày bắt đầu.");

            var project = new Repository.Models.Project
            {
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = (byte)ProjectStatus.NotStarted,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Projects.CreateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // Người tạo dự án tự động là thành viên đầu tiên
            await _unitOfWork.ProjectMembers.AddAsync(new ProjectMember
            {
                ProjectId = project.ProjectId,
                UserId = createdBy,
                JoinedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(project.ProjectId);
        }
        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int createdBy, bool creatorIsAdmin)
        {
            if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                throw new AppException("Ngày kết thúc không được trước ngày bắt đầu.");

            var project = new Repository.Models.Project
            {
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = (byte)ProjectStatus.NotStarted,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Projects.CreateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // adm tạo project để phân công, không phải là ng làm công việc.
            // mng tạo project thì tự động tham gia, vì họ là người trực tiếp làm việc.
            if (!creatorIsAdmin)
            {
                await _unitOfWork.ProjectMembers.AddAsync(new ProjectMember
                {
                    ProjectId = project.ProjectId,
                    UserId = createdBy,
                    JoinedDate = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync();
            }

            return await GetByIdAsync(project.ProjectId);
        }
        public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id)
                ?? throw new AppException("Dự án không tồn tại.");

            if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                throw new AppException("Ngày kết thúc không được trước ngày bắt đầu.");

            project.ProjectName = dto.ProjectName;
            project.Description = dto.Description;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.Status = dto.Status;

            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id)
                ?? throw new AppException("Dự án không tồn tại.");

            var tasks = await _unitOfWork.ProjectTasks.GetAllAsync();
            if (tasks.Any(t => t.ProjectId == id))
                throw new AppException("Không thể xóa dự án còn công việc. Hãy xóa hết công việc trước.");

            await _unitOfWork.Projects.DeleteAsync(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectMemberDto>> GetMembersAsync(int projectId)
        {
            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(projectId);
            return members.Select(m => new ProjectMemberDto
            {
                ProjectMemberId = m.ProjectMemberId,
                UserId = m.UserId,
                UserName = m.User.UserName,
                FullName = m.User.FullName,
                JoinedDate = m.JoinedDate ?? DateTime.MinValue
            });
        }

        public async Task<IEnumerable<UserDto>> GetAvailableUsersAsync(int projectId)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(projectId);
            var memberIds = members.Select(m => m.UserId).ToHashSet();

            return allUsers
                .Where(u => (u.IsActive ?? true) && !memberIds.Contains(u.UserAccountId))
                .Select(u => new UserDto
                {
                    UserAccountId = u.UserAccountId,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.Role?.RoleName ?? string.Empty,
                    IsActive = u.IsActive ?? true
                });
        }

        public async Task AddMemberAsync(int projectId, int userId)
        {
            _ = await _unitOfWork.Projects.GetByIdAsync(projectId)
                ?? throw new AppException("Dự án không tồn tại.");

            if (await IsMemberAsync(projectId, userId))
                throw new AppException("Người dùng đã là thành viên của dự án.");

            await _unitOfWork.ProjectMembers.AddAsync(new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId,
                JoinedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int projectId, int userId)
        {
            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(projectId);
            var membership = members.FirstOrDefault(m => m.UserId == userId)
                ?? throw new AppException("Người dùng không thuộc dự án này.");

            await _unitOfWork.ProjectMembers.RemoveAsync(membership);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsMemberAsync(int projectId, int userId)
        {
            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(projectId);
            return members.Any(m => m.UserId == userId);
        }

        private async Task<ProjectDto> MapToDtoAsync(Project project)
        {
            var members = await _unitOfWork.ProjectMembers.GetByProjectIdAsync(project.ProjectId);
            var tasks = await _unitOfWork.ProjectTasks.GetAllAsync();

            var status = project.Status ?? (byte)ProjectStatus.NotStarted;

            return new ProjectDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = status,
                StatusName = StatusLabels.ProjectStatusName((ProjectStatus)status),
                CreatedBy = project.CreatedBy,
                CreatedByName = project.CreatedByNavigation?.FullName ?? string.Empty,
                CreatedDate = project.CreatedDate ?? DateTime.MinValue,
                MemberCount = members.Count(),
                TaskCount = tasks.Count(t => t.ProjectId == project.ProjectId)
            };
        }
    }
}
