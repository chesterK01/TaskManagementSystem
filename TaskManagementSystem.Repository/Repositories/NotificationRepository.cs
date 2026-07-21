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
    public class NotificationRepository : INotificationRepository
    {
        private readonly TaskManagementSystemContext _context;

        public NotificationRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }
        public async Task<Notification?> GetByIdAsync(int id)
        => await _context.Notifications.FindAsync(id);
    }
}
