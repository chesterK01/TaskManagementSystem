namespace TaskManagementSystem.Web.Models
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string UserName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public bool CanDelete { get; set; }
    }
}
