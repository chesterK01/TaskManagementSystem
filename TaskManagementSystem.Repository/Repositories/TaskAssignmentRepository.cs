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
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly TaskManagementSystemContext _context;

        public TaskAssignmentRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskAssignment>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskAssignments
                .Include(x => x.User)
                .Where(x => x.TaskId == taskId)
                .ToListAsync();
        }

        public async Task AddAsync(TaskAssignment assignment)
        {
            await _context.TaskAssignments.AddAsync(assignment);
        }

        public Task RemoveAsync(TaskAssignment assignment)
        {
            _context.TaskAssignments.Remove(assignment);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<TaskAssignment>> GetByUserIdAsync(int userId)
        {
            return await _context.TaskAssignments
                .Include(x => x.Task)
                    .ThenInclude(t => t.Project)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
