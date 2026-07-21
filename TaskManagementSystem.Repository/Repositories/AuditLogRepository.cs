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
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly TaskManagementSystemContext _context;

        public AuditLogRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
