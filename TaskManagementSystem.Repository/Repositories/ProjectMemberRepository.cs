using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly TaskManagementSystemContext _context;

        public ProjectMemberRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProjectMembers
                .Include(x => x.User)
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectMember member)
        {
            await _context.ProjectMembers.AddAsync(member);
        }

        public Task RemoveAsync(ProjectMember member)
        {
            _context.ProjectMembers.Remove(member);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<ProjectMember>> GetByUserIdAsync(int userId)
        {
            return await _context.ProjectMembers
                .Include(x => x.Project)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
