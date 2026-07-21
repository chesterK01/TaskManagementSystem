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
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementSystemContext _context;

        public UserRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<SystemUserAccount?> GetByUserNameAsync(string username)
        {
            return await _context.SystemUserAccounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<SystemUserAccount?> GetByIdAsync(int id)
        {
            return await _context.SystemUserAccounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserAccountId == id);
        }

        public async Task<SystemUserAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemUserAccounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<SystemUserAccount>> GetAllAsync()
        {
            return await _context.SystemUserAccounts
                .Include(x => x.Role)
                .OrderBy(x => x.UserName)
                .ToListAsync();
        }

        public async Task CreateAsync(SystemUserAccount user)
        {
            await _context.SystemUserAccounts.AddAsync(user);
        }

        public Task UpdateAsync(SystemUserAccount user)
        {
            _context.SystemUserAccounts.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(SystemUserAccount user)
        {
            _context.SystemUserAccounts.Remove(user);
            return Task.CompletedTask;
        }

        public async Task<bool> UserNameExistsAsync(string username)
        {
            return await _context.SystemUserAccounts
                .AnyAsync(x => x.UserName == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.SystemUserAccounts
                .AnyAsync(x => x.Email == email);
        }
    }
}
