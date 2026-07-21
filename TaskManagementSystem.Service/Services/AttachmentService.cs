using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Attachment;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AttachmentDto>> GetByTaskIdAsync(int taskId)
        {
            var files = await _unitOfWork.Attachments.GetByTaskIdAsync(taskId);
            return files.Select(a => new AttachmentDto
            {
                AttachmentId = a.AttachmentId,
                TaskId = a.TaskId,
                FileName = a.FileName ?? string.Empty,
                FilePath = a.FilePath ?? string.Empty,
                UploadedByName = a.UploadedByNavigation?.FullName ?? string.Empty,
                UploadedDate = a.UploadedDate ?? DateTime.MinValue
            });
        }

        public async Task<AttachmentDto> AddAsync(int taskId, string fileName, string filePath, int uploadedBy)
        {
            var attachment = new Attachment
            {
                TaskId = taskId,
                FileName = fileName,
                FilePath = filePath,
                UploadedBy = uploadedBy,
                UploadedDate = DateTime.UtcNow
            };

            await _unitOfWork.Attachments.AddAsync(attachment);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Attachments.GetByIdAsync(attachment.AttachmentId)
                ?? throw new AppException("Tải file thất bại.");

            return new AttachmentDto
            {
                AttachmentId = created.AttachmentId,
                TaskId = created.TaskId,
                FileName = created.FileName ?? string.Empty,
                FilePath = created.FilePath ?? string.Empty,
                UploadedByName = created.UploadedByNavigation?.FullName ?? string.Empty,
                UploadedDate = created.UploadedDate ?? DateTime.MinValue
            };
        }

        public async Task<(string filePath, string fileName)> GetFileForDownloadAsync(int attachmentId)
        {
            var attachment = await _unitOfWork.Attachments.GetByIdAsync(attachmentId)
                ?? throw new AppException("File không tồn tại.");
            return (attachment.FilePath ?? string.Empty, attachment.FileName ?? "file");
        }

        public async Task<string> DeleteAsync(int attachmentId, int requesterId, bool requesterIsAdmin)
        {
            var attachment = await _unitOfWork.Attachments.GetByIdAsync(attachmentId)
                ?? throw new AppException("File không tồn tại.");

            if (attachment.UploadedBy != requesterId && !requesterIsAdmin)
                throw new AppException("Bạn không có quyền xóa file này.");

            var filePath = attachment.FilePath ?? string.Empty;
            await _unitOfWork.Attachments.RemoveAsync(attachment);
            await _unitOfWork.SaveChangesAsync();

            return filePath;
        }
    }
}
