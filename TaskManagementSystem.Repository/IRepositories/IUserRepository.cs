using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IUserRepository
    {
        Task<SystemUserAccount?> GetByUserNameAsync(string username);
        Task<SystemUserAccount?> GetByIdAsync(int id);

        Task<SystemUserAccount?> GetByEmailAsync(string email);

        Task<IEnumerable<SystemUserAccount>> GetAllAsync();

        Task CreateAsync(SystemUserAccount user);

        Task UpdateAsync(SystemUserAccount user);

        Task DeleteAsync(SystemUserAccount user);

        Task<bool> UserNameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);
    }
}
