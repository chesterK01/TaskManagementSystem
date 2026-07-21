using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Repository.Models;

namespace TaskManagementSystem.Repository.IRepositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(int id);

        Task<IEnumerable<ProjectTask>> GetAllAsync();

        Task CreateAsync(ProjectTask task);

        Task UpdateAsync(ProjectTask task);

        Task DeleteAsync(ProjectTask task);
    }
}
