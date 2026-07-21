namespace TaskManagementSystem.Web.Models
{
    public class ProjectDetailsViewModel
    {
        public ProjectViewModel Project { get; set; } = null!;
        public List<ProjectMemberViewModel> Members { get; set; } = new();
        public List<ProjectTaskViewModel> Tasks { get; set; } = new();
        public bool CanManage { get; set; } // Admin/Manager hoặc chủ project
    }
}
