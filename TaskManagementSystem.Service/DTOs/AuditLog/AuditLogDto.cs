using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.AuditLog
{
    public class AuditLogDto
    {
        public int AuditLogId { get; set; }
        public string? UserName { get; set; }
        public string TableName { get; set; } = null!;
        public int? RecordId { get; set; }
        public string Action { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
