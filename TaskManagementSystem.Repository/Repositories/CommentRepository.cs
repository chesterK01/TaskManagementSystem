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
    public class CommentRepository : ICommentRepository
    {
        private readonly TaskManagementSystemContext _context;

        public CommentRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Comments
                .Include(x => x.User)
                .Where(x => x.TaskId == taskId)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }
        public async Task<Comment?> GetByIdAsync(int id)
        => await _context.Comments.FindAsync(id);

        public Task RemoveAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Task.CompletedTask;
        }
    }
}
