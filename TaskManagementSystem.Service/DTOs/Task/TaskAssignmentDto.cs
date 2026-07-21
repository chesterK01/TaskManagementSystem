using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Task
{
    public class TaskAssignmentDto
    {
        public int TaskAssignmentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime AssignedDate { get; set; }
    }
}
