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
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoleDto>> GetRolesAsync();

    }
}
