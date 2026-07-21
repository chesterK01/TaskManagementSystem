using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : Controller
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _auditLogService.GetAllAsync();
            var viewModels = logs.Select(l => new AuditLogViewModel
            {
                UserName = l.UserName,
                TableName = l.TableName,
                RecordId = l.RecordId,
                Action = l.Action,
                CreatedDate = l.CreatedDate
            });
            return View(viewModels);
        }
    }
}
