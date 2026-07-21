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
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagementSystemContext _context;

        public ProjectRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(x => x.CreatedByNavigation)
                .FirstOrDefaultAsync(x => x.ProjectId == id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(x => x.CreatedByNavigation)
                .ToListAsync();
        }

        public async Task CreateAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            return Task.CompletedTask;
        }
    }
}
