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
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly TaskManagementSystemContext _context;

        public AttachmentRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Attachments
                .Where(x => x.TaskId == taskId)
                .ToListAsync();
        }

        public async Task AddAsync(Attachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
        }
        public async Task<Attachment?> GetByIdAsync(int id)
        => await _context.Attachments.FindAsync(id);

        public Task RemoveAsync(Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
            return Task.CompletedTask;
        }
    }
}
