using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs.Auth;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.User;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(dto.UserName);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new AppException("Tên đăng nhập hoặc mật khẩu không đúng.");

            if (!(user.IsActive ?? true))
                throw new AppException("Tài khoản đã bị khóa.");

            return new UserDto
            {
                UserAccountId = user.UserAccountId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName ?? string.Empty,
                IsActive = user.IsActive ?? true
            };
        }
    }
}