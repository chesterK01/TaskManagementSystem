using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task AddAsync(Notification notification);
        Task<Notification?> GetByIdAsync(int id);
    }
}
