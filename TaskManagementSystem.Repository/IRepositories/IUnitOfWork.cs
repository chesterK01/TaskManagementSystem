using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IRoleRepository Roles { get; }

        IProjectRepository Projects { get; }

        IProjectMemberRepository ProjectMembers { get; }

        IProjectTaskRepository ProjectTasks { get; }

        ITaskAssignmentRepository TaskAssignments { get; }

        ICommentRepository Comments { get; }

        IAttachmentRepository Attachments { get; }

        INotificationRepository Notifications { get; }

        IAuditLogRepository AuditLogs { get; }

        ITaskHistoryRepository TaskHistories { get; }

        Task<int> SaveChangesAsync();
    }
}
