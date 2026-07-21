using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Comment;

namespace TaskManagementSystem.Service.IServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetByTaskIdAsync(int taskId);
        Task AddAsync(int taskId, int userId, string content);
        Task DeleteAsync(int commentId, int requesterId, bool requesterIsAdmin);
    }
}
