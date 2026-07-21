using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId);
        Task AddAsync(Comment comment);
        Task<Comment?> GetByIdAsync(int id);
        Task RemoveAsync(Comment comment);
    }
}
