using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Project
{
    public class ProjectMemberDto
    {
        public int ProjectMemberId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime JoinedDate { get; set; }
    }
}
