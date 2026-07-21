using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IProjectMemberRepository
    {
        Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(int projectId);
        Task AddAsync(ProjectMember member);
        Task RemoveAsync(ProjectMember member);
        Task<IEnumerable<ProjectMember>> GetByUserIdAsync(int userId);
    }
}
