using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Task
{
    public class UpdateProjectTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte Priority { get; set; }
        public DateOnly? DueDate { get; set; }
    }
}
