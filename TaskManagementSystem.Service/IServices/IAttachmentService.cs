using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Attachment;

namespace TaskManagementSystem.Service.IServices
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentDto>> GetByTaskIdAsync(int taskId);
        Task<AttachmentDto> AddAsync(int taskId, string fileName, string filePath, int uploadedBy);
        Task<(string filePath, string fileName)> GetFileForDownloadAsync(int attachmentId);
        Task<string> DeleteAsync(int attachmentId, int requesterId, bool requesterIsAdmin);
    }
}
