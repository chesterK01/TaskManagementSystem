using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}
