using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementSystemContext _context;

        private IUserRepository? _userRepository;
        private IRoleRepository? _roleRepository;
        private IProjectRepository? _projectRepository;
        private IProjectMemberRepository? _projectMemberRepository;
        private IProjectTaskRepository? _projectTaskRepository;
        private ITaskAssignmentRepository? _taskAssignmentRepository;
        private ICommentRepository? _commentRepository;
        private IAttachmentRepository? _attachmentRepository;
        private INotificationRepository? _notificationRepository;
        private IAuditLogRepository? _auditLogRepository;
        private ITaskHistoryRepository? _taskHistoryRepository;

        public UnitOfWork(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public IUserRepository Users
            => _userRepository ??= new UserRepository(_context);
        public IRoleRepository Roles
            => _roleRepository ??= new RoleRepository(_context);
        public IProjectRepository Projects
            => _projectRepository ??= new ProjectRepository(_context);

        public IProjectMemberRepository ProjectMembers
            => _projectMemberRepository ??= new ProjectMemberRepository(_context);

        public IProjectTaskRepository ProjectTasks
            => _projectTaskRepository ??= new ProjectTaskRepository(_context);

        public ITaskAssignmentRepository TaskAssignments
            => _taskAssignmentRepository ??= new TaskAssignmentRepository(_context);

        public ICommentRepository Comments
            => _commentRepository ??= new CommentRepository(_context);

        public IAttachmentRepository Attachments
            => _attachmentRepository ??= new AttachmentRepository(_context);

        public INotificationRepository Notifications
            => _notificationRepository ??= new NotificationRepository(_context);

        public IAuditLogRepository AuditLogs
            => _auditLogRepository ??= new AuditLogRepository(_context);

        public ITaskHistoryRepository TaskHistories
            => _taskHistoryRepository ??= new TaskHistoryRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}