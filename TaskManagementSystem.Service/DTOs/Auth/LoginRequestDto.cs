using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
