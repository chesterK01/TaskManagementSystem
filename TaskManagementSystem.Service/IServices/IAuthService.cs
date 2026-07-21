using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Service.DTOs.Auth;
using TaskManagementSystem.Service.DTOs.User;

namespace TaskManagementSystem.Service.IServices
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginRequestDto dto);

    }
}

