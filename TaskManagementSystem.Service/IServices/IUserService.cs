using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.User;

namespace TaskManagementSystem.Service.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto, int performedBy);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto, int performedBy);
        Task DeleteAsync(int id, int performedBy);
        Task<IEnumerable<RoleDto>> GetRolesAsync();

    }
}
