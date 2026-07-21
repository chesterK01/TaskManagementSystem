namespace TaskManagementSystem.Web.Models
{
    public class TaskAssignmentViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime AssignedDate { get; set; }
    }
}
