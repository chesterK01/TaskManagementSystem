using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.User
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}