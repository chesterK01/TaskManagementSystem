using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.AuditLog;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
        {
            var logs = await _unitOfWork.AuditLogs.GetAllAsync();
            return logs.Select(l => new AuditLogDto
            {
                AuditLogId = l.AuditLogId,
                UserName = l.User?.FullName,
                TableName = l.TableName ?? string.Empty,
                RecordId = l.RecordId,
                Action = l.Action ?? string.Empty,
                CreatedDate = l.CreatedDate ?? DateTime.MinValue
            });
        }
        public async Task LogAsync(int? userId, string tableName, int? recordId, string action)
        {
            await _unitOfWork.AuditLogs.AddAsync(new AuditLog
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Action = action,
                CreatedDate = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
