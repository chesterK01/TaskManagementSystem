using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Attachment;
using TaskManagementSystem.Service.DTOs.Comment;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CommentDto>> GetByTaskIdAsync(int taskId)
        {
            var comments = await _unitOfWork.Comments.GetByTaskIdAsync(taskId);
            return comments.Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                TaskId = c.TaskId,
                UserId = c.UserId,
                UserName = c.User?.FullName ?? string.Empty,
                Content = c.Content,
                CreatedDate = c.CreatedDate ?? DateTime.MinValue
            });
        }

        public async Task AddAsync(int taskId, int userId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new AppException("Nội dung bình luận không được để trống.");

            await _unitOfWork.Comments.AddAsync(new Comment
            {
                TaskId = taskId,
                UserId = userId,
                Content = content.Trim(),
                CreatedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int commentId, int requesterId, bool requesterIsAdmin)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId)
                ?? throw new AppException("Bình luận không tồn tại.");

            if (comment.UserId != requesterId && !requesterIsAdmin)
                throw new AppException("Bạn không có quyền xóa bình luận này.");

            await _unitOfWork.Comments.RemoveAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}