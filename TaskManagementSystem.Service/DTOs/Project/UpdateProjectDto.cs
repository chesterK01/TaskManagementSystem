using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Project
{
    public class UpdateProjectDto
    {
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public byte Status { get; set; }
    }
}
