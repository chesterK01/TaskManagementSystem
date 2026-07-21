namespace TaskManagementSystem.Web.Models
{
    public class ProjectMemberViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime JoinedDate { get; set; }
    }
}
