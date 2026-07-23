using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.AuditLog;

namespace TaskManagementSystem.Service.IServices
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLogDto>> GetAllAsync();
        Task LogAsync(int? userId, string tableName, int? recordId, string action);
    }
}
