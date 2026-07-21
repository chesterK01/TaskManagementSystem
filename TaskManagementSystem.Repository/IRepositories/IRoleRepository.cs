using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IRoleRepository
    {
        Task<SystemRole?> GetByIdAsync(int id);
        Task<IEnumerable<SystemRole>> GetAllAsync();
    }
}
