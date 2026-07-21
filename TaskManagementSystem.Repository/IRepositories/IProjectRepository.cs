using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);

        Task<IEnumerable<Project>> GetAllAsync();

        Task CreateAsync(Project project);

        Task UpdateAsync(Project project);

        Task DeleteAsync(Project project);
    }
}
