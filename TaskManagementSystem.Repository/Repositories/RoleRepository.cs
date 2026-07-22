using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TaskManagementSystemContext _context;

        public RoleRepository(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<SystemRole?> GetByIdAsync(int id)
        {
            return await _context.SystemRoles.FindAsync(id);
        }

        public async Task<IEnumerable<SystemRole>> GetAllAsync()
        {
            return await _context.SystemRoles.ToListAsync();
        }
    }
}