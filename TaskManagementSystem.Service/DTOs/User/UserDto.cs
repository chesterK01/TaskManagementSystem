using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.User
{
    public class UserDto
    {
        public int UserAccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
