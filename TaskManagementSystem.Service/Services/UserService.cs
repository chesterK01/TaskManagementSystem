using TaskManagementSystem.Repository.IRepositories;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Service.DTOs;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.User;
using TaskManagementSystem.Service.IServices;

namespace TaskManagementSystem.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new AppException("Người dùng không tồn tại.");
            return MapToDto(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (await _unitOfWork.Users.UserNameExistsAsync(dto.UserName))
                throw new AppException("Tên đăng nhập đã tồn tại.");

            if (await _unitOfWork.Users.EmailExistsAsync(dto.Email))
                throw new AppException("Email đã được sử dụng.");

            var role = await _unitOfWork.Roles.GetByIdAsync(dto.RoleId)
                ?? throw new AppException("Vai trò (Role) không hợp lệ.");

            var user = new SystemUserAccount
            {
                UserName = dto.UserName,
                // Mật khẩu do cty cấp, có thể bắt đổi ở lần đăng nhập đầu
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                RoleId = dto.RoleId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Users.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Users.GetByIdAsync(user.UserAccountId)
                ?? throw new AppException("Tạo tài khoản thất bại.");

            return MapToDto(created);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new AppException("Người dùng không tồn tại.");

            var role = await _unitOfWork.Roles.GetByIdAsync(dto.RoleId)
                ?? throw new AppException("Vai trò (Role) không hợp lệ.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;
            user.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new AppException("Người dùng không tồn tại.");


            user.IsActive = false;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return roles.Select(r => new RoleDto { RoleId = r.RoleId, RoleName = r.RoleName });
        }
        private static UserDto MapToDto(SystemUserAccount user) => new()
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