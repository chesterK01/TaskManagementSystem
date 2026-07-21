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
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly TaskManagementSystemContext _context;

        public ProjectTaskRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask?> GetByIdAsync(int id)
        {
            return await _context.ProjectTasks
                .Include(x => x.Project)
                .Include(x => x.CreatedByNavigation)
                .FirstOrDefaultAsync(x => x.TaskId == id);
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTasks
                .Include(x => x.Project)
                .Include(x => x.CreatedByNavigation)
                .ToListAsync();
        }

        public async Task CreateAsync(ProjectTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
        }

        public Task UpdateAsync(ProjectTask task)
        {
            _context.ProjectTasks.Update(task);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ProjectTask task)
        {
            _context.ProjectTasks.Remove(task);
            return Task.CompletedTask;
        }
    }
}
