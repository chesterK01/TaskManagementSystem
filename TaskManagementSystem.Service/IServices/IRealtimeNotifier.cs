using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.IServices
{
    public interface IRealtimeNotifier
    {
        Task NotifyUserAsync(int userId, string title, string content);
    }
}
