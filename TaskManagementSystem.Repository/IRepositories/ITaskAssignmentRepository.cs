using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface ITaskAssignmentRepository
    {
        Task<IEnumerable<TaskAssignment>> GetByTaskIdAsync(int taskId);
        Task AddAsync(TaskAssignment assignment);
        Task RemoveAsync(TaskAssignment assignment);
        Task<IEnumerable<TaskAssignment>> GetByUserIdAsync(int userId);
    }
}
