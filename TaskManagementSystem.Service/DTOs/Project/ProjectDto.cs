using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Project
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int MemberCount { get; set; }
        public int TaskCount { get; set; }
    }
}
