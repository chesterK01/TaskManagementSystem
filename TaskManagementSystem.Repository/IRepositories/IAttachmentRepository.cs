using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IAttachmentRepository
    {
        Task<IEnumerable<Attachment>> GetByTaskIdAsync(int taskId);
        Task AddAsync(Attachment attachment);
        Task<Attachment?> GetByIdAsync(int id);
        Task RemoveAsync(Attachment attachment);
    }
}
