namespace TaskManagementSystem.Web.Models
{
    public class UserViewModel
    {
        public int UserAccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
