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
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly TaskManagementSystemContext _context;

        public TaskHistoryRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskHistory history)
        {
            await _context.TaskHistories.AddAsync(history);
        }

        public async Task<IEnumerable<TaskHistory>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskHistories
                .Where(x => x.TaskId == taskId)
                .OrderByDescending(x => x.ChangedDate)
                .ToListAsync();
        }
    }
}
