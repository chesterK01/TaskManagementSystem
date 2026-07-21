namespace TaskManagementSystem.Web.Models
{
    public class ProjectTaskViewModel
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte Priority { get; set; }
        public string PriorityName { get; set; } = null!;
        public byte Status { get; set; }
        public string StatusName { get; set; } = null!;
        public DateOnly? DueDate { get; set; }
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public List<TaskAssignmentViewModel> AssignedUsers { get; set; } = new();
    }
}
